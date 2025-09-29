// ApiClientService.cs
using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzel.Integration.interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EM.Comax.ShukHerzel.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using EM.Comax.ShukHerzl.Common;
using EM.Comax.ShukHerzel.Models.CustomModels;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class ApiClientService : IApiClientService
    {
        private readonly IItemsRepository _itemRepository;
        private readonly IEslApiClient _eslApiClient;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly ILogger<ApiClientService> _logger;
        private readonly IApiConfigService _apiConfigService;
        private readonly ITrailingItemRepository _trailingItemRepository;
        private readonly int _batchSize;

        public ApiClientService(
            IItemsRepository itemRepository,
            IEslApiClient eslApiClient,
            IDatabaseLogger databaseLogger,
            ILogger<ApiClientService> logger,
            IApiConfigService apiConfigService,
            ITrailingItemRepository trailingItemRepository
            )
        {
            _itemRepository = itemRepository;
            _eslApiClient = eslApiClient;
            _databaseLogger = databaseLogger;
            _logger = logger;
            _apiConfigService = apiConfigService;
            _trailingItemRepository = trailingItemRepository;

            // Retrieve batch size from configuration with a default value
            _batchSize = Constants.ESL_BATCH_SIZE;
        }

        /// <summary>
        /// Sends items to ESL API in batches with reporting and logging.
        /// Groups items by StoreId and sends each group to the corresponding store endpoint.
        /// </summary>
        /// <param name="logRequests">Whether to log the requests and responses.</param>
        /// <param name="progress">Progress reporter for real-time updates.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendItemsToEslAsync(bool logRequests, IProgress<string> progress, CancellationToken cancellationToken = default)
        {
            try
            {
                var now = DateTime.Now;
                await _databaseLogger.LogServiceActionAsync("ApiClientService: Starting to send items to the ESL API.");
                progress.Report("Starting synchronization of Items to ESL API...");

                // 1. Fetch items to send
                var items = await _itemRepository.GetItemWithBranches();
                int totalItems = items.Count();
                progress.Report($"Fetched {totalItems} items to send.");

                if (!items.Any())
                {
                    progress.Report("No items to send.");
                    await _databaseLogger.LogServiceActionAsync("ApiClientService: No items to send.");
                    return;
                }

                // 2. Fetch trailing items for items that have trailing item references
                var trailingItemKods = items
                    .Where(item => !string.IsNullOrWhiteSpace(item.Item.TrailingItem) && item.Item.TrailingItem != "0")
                    .Select(item => item.Item.TrailingItem)
                    .Distinct()
                    .ToList();

                Dictionary<string, TrailingItem> trailingItems = new Dictionary<string, TrailingItem>();
                if (trailingItemKods.Any())
                {
                    trailingItems = await _trailingItemRepository.GetByKodsAsync(trailingItemKods);
                    progress.Report($"Fetched {trailingItems.Count} trailing items from database.");
                    await _databaseLogger.LogServiceActionAsync($"ApiClientService: Fetched {trailingItems.Count} trailing items for {trailingItemKods.Count} requested Kods.");
                }

                // 3. Map items to EslDto
                var eslDtos = new List<EslDto>();
                foreach (var item in items)
                {
                    // Always create DTO with Barcode as id
                    eslDtos.Add(MapToDto(item, trailingItems, useXmlIdAsId: false));
                    
                    // If XmlId is different from Barcode, create another DTO with XmlId as id
                    if (!string.IsNullOrEmpty(item.Item.XmlId) && 
                        item.Item.XmlId != item.Item.Barcode)
                    {
                        eslDtos.Add(MapToDto(item, trailingItems, useXmlIdAsId: true));
                    }
                }
                progress.Report($"Mapped {eslDtos.Count} items to ESL DTOs (including XmlId variants).");

                // 3. Group items by StoreId
                var groupedItems = eslDtos.GroupBy(dto => dto.custom.StoreId);
                int totalGroups = groupedItems.Count();
                progress.Report($"Grouped items by StoreId into {totalGroups} groups.");

                int currentGroup = 0;

                // 4. Iterate through each group and send batches
                foreach (var group in groupedItems)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    currentGroup++;

                    string storeId = group.Key;
                    var storeItems = group.ToList();

                    progress.Report($"Processing StoreId {storeId} with {storeItems.Count} items.");
                    await _databaseLogger.LogServiceActionAsync($"ApiClientService: Processing StoreId {storeId} with {storeItems.Count} items.");

                    // Split store items into batches
                    var batches = SplitIntoBatches(storeItems, _batchSize);
                    int totalBatches = batches.Count;
                    int currentBatch = 0;

                    foreach (var batch in batches)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        currentBatch++;

                        string batchInfo = $"StoreId {storeId} - Batch {currentBatch}/{totalBatches} with {batch.Count} items.";
                        progress.Report($"Sending {batchInfo}");
                        await _databaseLogger.LogServiceActionAsync($"ApiClientService: Sending {batchInfo}");

                        try
                        {
                            // Send the batch to the ESL API with the storeId
                            await _eslApiClient.SendItemsToEslAsync(storeId, batch, cancellationToken);
                            progress.Report($"Successfully sent {batchInfo}");
                            await _databaseLogger.LogServiceActionAsync($"ApiClientService: Successfully sent {batchInfo}");

                            // Mark items as transferred
                            var itemIds = items
                                .Where(item => batch.Any(dto => dto.id == item.Item.Barcode))
                                .Select(item => item.Item.Id)
                                .ToList();

                            if (itemIds.Any())
                            {
                                await _itemRepository.MarkAsTransferredAsync(itemIds, now);
                                progress.Report($"Marked {itemIds.Count} items as transferred.");
                                await _databaseLogger.LogServiceActionAsync($"ApiClientService: Marked {itemIds.Count} items as transferred.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log error and continue with next batch
                            progress.Report($"Error sending {batchInfo}: {ex.Message}");
                            await _databaseLogger.LogErrorAsync("ApiClientService", $"Error sending {batchInfo}", ex);
                            _logger.LogError(ex, $"ApiClientService: Error sending {batchInfo}");

                            // Depending on requirements, you might want to re-throw or continue
                            // throw;
                            continue;
                        }
                    }
                }

                await _databaseLogger.LogServiceActionAsync("ApiClientService: Finished sending items to the ESL API.");
                progress.Report("Synchronization completed successfully.");
            }
            catch (OperationCanceledException)
            {
                await _databaseLogger.LogServiceActionAsync("ApiClientService: Synchronization was canceled.");
                progress.Report("Synchronization was canceled.");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("ApiClientService", "Error in SendItemsToEslAsync", ex);
                _logger.LogError(ex, "ApiClientService: Error in SendItemsToEslAsync");
                progress.Report($"Error during synchronization: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps an ItemWithBranch entity to an EslDto.
        /// </summary>
        /// <param name="itemWithBranch">The item with branch information to map.</param>
        /// <param name="trailingItems">Dictionary of trailing items by Kod.</param>
        /// <param name="useXmlIdAsId">If true, use XmlId as id; otherwise use Barcode.</param>
        /// <returns>The mapped EslDto.</returns>
        private EslDto MapToDto(ItemWithBranch itemWithBranch, Dictionary<string, TrailingItem> trailingItems, bool useXmlIdAsId = false)
        {
            var item = itemWithBranch.Item;
            
            // Calculate trailing item prices
            var trailingItemPrice = CalculateTrailingItemPrice(item, trailingItems);
            var trailingItemPromotionPrice = CalculateTrailingItemPromotionPrice(item, trailingItems);
            
            return new EslDto
            {
                brand = string.Empty, // Adjust as necessary
                id = useXmlIdAsId ? (item.XmlId?.Trim() ?? string.Empty) : (item.Barcode?.Trim() ?? string.Empty),
                name = item.Name?.Trim() ?? string.Empty,
                price = item.Price,
                
                custom = new Custom
                {
                    ManufacturingCountry = item.ManufacturingCountry?.Trim() ?? string.Empty,
                    Content = item.Content?.Trim() ?? string.Empty,
                    ContentUnit = item.ContentUnit?.Trim() ?? string.Empty,
                    ContentMeasure = item.ContentMeasure?.Trim() ?? string.Empty,
                    Quantity = item.Quantity?.ToString() ?? string.Empty,
                    Total = item.TotalPromotionPrice?.ToString() ?? string.Empty,
                    FromDate = ParseDate(item.PromotionFromDate),
                    ToDate = ParseDate(item.PromotionToDate),
                    Kod = item.PromotionKod?.Trim() ?? string.Empty,
                    SwAllCustomers = item.SwAllCustomers.HasValue && item.SwAllCustomers.Value ? "true" : "false",
                    SwWeighable = item.SwWeighable.HasValue && item.SwWeighable.Value ? "true" : "false",
                    TextForWeb = item.TextForWeb?.Trim() ?? string.Empty,
                    StoreId = itemWithBranch.StoreId, // Ensure StoreId is captured if needed in DTO
                    AllBarcodes = item.PromotionBarcodes ?? "",
                    Size = item.Size?.ToString() ?? string.Empty,
                    IsPromotion = item.IsPromotion.ToString() ?? "False",
                    SwPikadon = item.SwPikadon?.ToString() ?? "False",
                    TrailingItem = item.TrailingItem?.ToString() ?? "",
                    TotalForActivate = item.TotalForActivate?.ToString() ?? string.Empty,
                    PromotionQuantity = item.PromotionQuantity?.ToString() ?? string.Empty,
                    GetDiscountTotal = item.GetDiscountTotal?.ToString() ?? string.Empty,
                    GetCmt = item.GetCmt?.Trim() ?? string.Empty,
                    GetDiscountPrecent = item.GetDiscountPrecent?.ToString() ?? string.Empty,
                    GetTotal = item.GetTotal?.ToString() ?? string.Empty,
                    PromotionMinQty = item.PromotionMinQty?.ToString() ?? string.Empty,
                    PromotionMaxQty = FormatPromotionMaxQty(item.PromotionMaxQty),
                    TrailingItemPrice = trailingItemPrice,
                    TrailingItemPromotionPrice = trailingItemPromotionPrice,
                    RemarkForPrint = item.RemarkForPrint ?? string.Empty

                }
            };
        }

        /// <summary>
        /// Parses a DateTime? to a string in "yyyy-MM-dd" format.
        /// Returns an empty string if the date is null.
        /// </summary>
        /// <param name="date">The date to parse.</param>
        /// <returns>Formatted date string or empty string.</returns>
        private string ParseDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : string.Empty;
        }

        /// <summary>
        /// Formats PromotionMaxQty with special rounding logic:
        /// - If decimal part < 0.5: round down to the unit (e.g., 3.4 → "3")
        /// - If decimal part >= 0.5: make it X.5 (e.g., 3.6 → "3.5", 3.5 → "3.5")
        /// </summary>
        /// <param name="maxQty">The MaxQty string value from the promotion.</param>
        /// <returns>Formatted MaxQty string for ESL API.</returns>
        private string FormatPromotionMaxQty(string maxQty)
        {
            if (string.IsNullOrWhiteSpace(maxQty))
                return string.Empty;

            // Try to parse the MaxQty as a decimal
            if (decimal.TryParse(maxQty.Trim(), out decimal value))
            {
                // Get the integer part and decimal part
                decimal integerPart = Math.Floor(value);
                decimal decimalPart = value - integerPart;

                if (decimalPart < 0.5m)
                {
                    // Round down to the unit (remove decimal part)
                    return integerPart.ToString("0");
                }
                else
                {
                    // Set to X.5 (integer part + 0.5)
                    return (integerPart + 0.5m).ToString("0.0");
                }
            }

            // If parsing fails, return original value
            return maxQty.Trim();
        }

        /// <summary>
        /// Calculates the trailing item price based on the item's TrailingItem reference.
        /// </summary>
        /// <param name="item">The item to calculate trailing price for.</param>
        /// <param name="trailingItems">Dictionary of trailing items by Kod.</param>
        /// <returns>Trailing item price as string, or empty string if not applicable.</returns>
        private string CalculateTrailingItemPrice(Models.Models.Item item, Dictionary<string, TrailingItem> trailingItems)
        {
            // If item has no trailing item reference or it's "0", return empty
            if (string.IsNullOrWhiteSpace(item.TrailingItem) || item.TrailingItem == "0")
                return string.Empty;

            // Look up the trailing item in the dictionary
            if (trailingItems.TryGetValue(item.TrailingItem, out var trailingItem))
            {
                // Return the price from the trailing item table
                return trailingItem.Price?.ToString() ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Calculates the trailing item promotion price by multiplying the trailing item price 
        /// with the promotion quantity if the item has a promotion.
        /// </summary>
        /// <param name="item">The item to calculate trailing promotion price for.</param>
        /// <param name="trailingItems">Dictionary of trailing items by Kod.</param>
        /// <returns>Trailing item promotion price as string, or empty string if not applicable.</returns>
        private string CalculateTrailingItemPromotionPrice(Models.Models.Item item, Dictionary<string, TrailingItem> trailingItems)
        {
            // If item has no promotion, return empty
            if (!item.IsPromotion.HasValue || !item.IsPromotion.Value)
                return string.Empty;

            // If item has no trailing item reference or it's "0", return empty
            if (string.IsNullOrWhiteSpace(item.TrailingItem) || item.TrailingItem == "0")
                return string.Empty;

            // Look up the trailing item in the dictionary
            if (trailingItems.TryGetValue(item.TrailingItem, out var trailingItem))
            {
                // Get the trailing item price
                var trailingPrice = trailingItem.Price;
                if (!trailingPrice.HasValue)
                    return string.Empty;

                // Get the promotion quantity
                if (decimal.TryParse(item.PromotionQuantity, out var promotionQty) && promotionQty > 0)
                {
                    // Multiply trailing item price by promotion quantity
                    var result = trailingPrice.Value * promotionQty;
                    return result.ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Splits a list into smaller batches.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="batchSize">The maximum size of each batch.</param>
        /// <returns>A list of batches.</returns>
        private List<List<T>> SplitIntoBatches<T>(List<T> items, int batchSize)
        {
            var batches = new List<List<T>>();
            for (int i = 0; i < items.Count; i += batchSize)
            {
                batches.Add(items.GetRange(i, Math.Min(batchSize, items.Count - i)));
            }
            return batches;
        }
    }
}
