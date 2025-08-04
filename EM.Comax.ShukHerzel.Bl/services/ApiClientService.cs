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
        private readonly int _batchSize;

        public ApiClientService(
            IItemsRepository itemRepository,
            IEslApiClient eslApiClient,
            IDatabaseLogger databaseLogger,
            ILogger<ApiClientService> logger,
            IApiConfigService apiConfigService
            )
        {
            _itemRepository = itemRepository;
            _eslApiClient = eslApiClient;
            _databaseLogger = databaseLogger;
            _logger = logger;
            _apiConfigService = apiConfigService;

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

                // 2. Map items to EslDto
                var eslDtos = items.Select(MapToDto).ToList();
                progress.Report($"Mapped {eslDtos.Count} items to ESL DTOs.");

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
        /// <returns>The mapped EslDto.</returns>
        private EslDto MapToDto(ItemWithBranch itemWithBranch)
        {
            var item = itemWithBranch.Item;
            return new EslDto
            {
                brand = string.Empty, // Adjust as necessary
                id = item.Barcode?.Trim() ?? string.Empty,
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
                    PromotionMaxQty = item.PromotionMaxQty?.ToString() ?? string.Empty

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
