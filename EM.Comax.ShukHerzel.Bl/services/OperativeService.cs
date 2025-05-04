using EFCore.BulkExtensions;
using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class OperativeService : IOperativeService
    {
        private readonly IAllItemsRepository _allItemsRepo;
        private readonly IPromotionsRepository _promoRepo;
        private readonly IItemsRepository _itemRepo;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IBadItemLogRepository _badItemLogRepository;
        private readonly IPriceUpdateRepository _priceUpdateRepository;
        private readonly IBranchRepository _branchRepository;

        // You may have a constant defined for the batch size
        private const int BatchSize = 1000;

        public OperativeService(
            IAllItemsRepository allItemsRepo,
            IPromotionsRepository promoRepo,
            IItemsRepository itemRepo,
            IDatabaseLogger databaseLogger,
            IBadItemLogRepository badItemLogRepository,
            IPriceUpdateRepository priceUpdateRepository,
            IBranchRepository branchRepository)
        {
            _allItemsRepo = allItemsRepo;
            _promoRepo = promoRepo;
            _itemRepo = itemRepo;
            _databaseLogger = databaseLogger;
            _badItemLogRepository = badItemLogRepository;
            _priceUpdateRepository = priceUpdateRepository;
            _branchRepository = branchRepository;
        }

        /// <summary>
        /// Synchronizes Items, Promotions, and Price Updates.
        /// </summary>
        public async Task SyncAllItemsAndPromotionsAsync(IProgress<string> progress, CancellationToken cancellationToken = default)
        {
            try
            {
                await _databaseLogger.LogServiceActionAsync("Starting SyncAllItemsAndPromotions...");
                progress.Report("Starting synchronization of Items, Promotions, and Price Updates...");

                var now = DateTime.Now; // Consider using UTC if appropriate

                // ---------------------------
                // ROUND 1: Process Catalog (AllItems) into Items (Upsert)
                // ---------------------------

                progress.Report("Removing duplicate operative Items...");
                await _itemRepo.CleanExpiredPromotions();
                await _allItemsRepo.RemoveDuplicateItemsAsync();

                var allItems = await _allItemsRepo.GetNonTransferredItemsAsync();
                progress.Report($"Fetched {allItems.Count} Items from temp table.");

                // Filter out items with zero price
                var itemsWithZeroPrice = allItems.Where(item => item.Price == "0").ToList();
                if (itemsWithZeroPrice.Any())
                {
                    progress.Report($"Found {itemsWithZeroPrice.Count} Items with zero price. Logging to BadItemLog...");
                    var badItemLogs = itemsWithZeroPrice.Select(item => new BadItemLog
                    {
                        Barcode = item.Barcode ?? "",
                        Message = "Price is zero.",
                        Guid = Guid.NewGuid().ToString(),
                        TimeStamp = DateTime.Now
                    }).ToList();

                    await _badItemLogRepository.BulkInsertBadItemsAsync(badItemLogs, BatchSize);
                    progress.Report($"Logged {badItemLogs.Count} bad items.");
                    var zeroPriceIds = itemsWithZeroPrice.Select(x => x.Id).ToList();
                    await _allItemsRepo.MarkAsBad(zeroPriceIds);
                    allItems = allItems.Except(itemsWithZeroPrice).ToList();
                }

                // Map AllItems to operative Items (upsert mapping)
                var operativeItems = allItems.Select(item => MapItem(item, now)).ToList();

                // Use BulkInsertOrUpdate to upsert items based on Barcode and BranchId.
                var bulkConfigOnlyItem = new BulkConfig
                {
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true,
                    BatchSize = Constants.OPERATIVE_BATCH_SIZE,
                    NotifyAfter = 1000,
                    UpdateByProperties = new List<string> { "Barcode", "BranchId" },
                    PropertiesToInclude = new List<string>
    {
        "Name",
        "Price",
        "CreateDateTime",
        "OperationGuid",
        "IsSentToEsl",
        "Content",
        "ContentMeasure",
        "ContentUnit",
        "Size",
        "SwWeighable",
        "ManufacturingCountry",
        "SwPikadon",
        "TrailingItem"
        // Add "Quantity" here if you want it updated as well.
    }
                };

                var bulkConfig = new BulkConfig
                {
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true,
                    BatchSize = Constants.OPERATIVE_BATCH_SIZE,
                    NotifyAfter = 1000,
                    UpdateByProperties = new List<string> { "Barcode", "BranchId" },
                    
                };

                progress.Report("Starting bulk upsert of Items from Catalog...");
                await _itemRepo.BulkInsertOrUpdateAsync(operativeItems, bulkConfigOnlyItem);
                progress.Report("Bulk upsert of Items completed.");
                var allItemIds = allItems.Select(a => a.Id).ToList();
                await _allItemsRepo.MarkAsTransferredAsync(allItemIds, now);
                progress.Report("Marked AllItems as transferred.");

                // ---------------------------
                // ROUND 2: Process Promotions (existing logic)
                // ---------------------------
                await _promoRepo.DeleteExpiredPromotionsAsync();
                var promos = await _promoRepo.GetNonTransferredPromotionsAsync();
                if (promos.Any())
                {
                    progress.Report($"Processing {promos.Count} Promotions...");
                    var promoKeys = promos.Select(p => (p.ItemKod, p.BranchId)).Distinct().ToList();
                    progress.Report("Fetching matching Items for Promotions...");
                    var matchingItems = await _itemRepo.GetItemsByBarcodesAndBranchIdsAsync(promoKeys);
                    progress.Report($"Found {matchingItems.Count} matching Items for Promotions.");

                    // Group items by key to identify duplicates
                    var groupedItems = matchingItems.GroupBy(i => (i.Barcode, i.BranchId)).ToList();

                    // Log warnings for duplicates separately
                    foreach (var group in groupedItems)
                    {
                        if (group.Count() > 1)
                        {
                            await _databaseLogger.LogServiceActionAsync($"Warning: Duplicate key ({group.Key.Barcode}, {group.Key.BranchId}) found in Items table during promotion processing. Using the first occurrence.");
                            progress.Report($"Warning: Duplicate key ({group.Key.Barcode}, {group.Key.BranchId}) found in Items table. Check data integrity.");
                        }
                    }

                    // Create the dictionary using the first item from each group
                    var itemDict = groupedItems.ToDictionary(g => g.Key, g => g.First());

                    var itemsToUpdate = new List<Models.Models.Item>();
                    var promoIdsToMark = new List<long>();
                    int promoCount = 0;
                    foreach (var promo in promos)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        try
                        {
                            var key = (promo.ItemKod, promo.BranchId);
                            if (itemDict.TryGetValue(key, out var item))
                            {
                                if (promo.SwActive?.ToLower() != "true")
                                {
                                    // Remove promotion details
                                    item.PromotionKod = null;
                                    item.PromotionFromDate = null;
                                    item.PromotionToDate = null;
                                    item.TotalPromotionPrice = null;
                                    item.SwAllCustomers = null;
                                    item.TextForWeb = null;
                                    item.Quantity = null;
                                    item.PromotionBarcodes = null;
                                    item.IsPromotion = false;
                                    item.IsSentToEsl = false;
                                }
                                else
                                {
                                    // Update Item with Promotion details
                                    item.PromotionKod = promo.Kod ?? "";
                                    item.PromotionFromDate = ParseDate(promo.FromDate);
                                    item.PromotionToDate = ParseDate(promo.ToDate);
                                    item.TotalPromotionPrice = TryParseDecimal(promo.Total);
                                    item.SwAllCustomers = promo.SwAllCustomers?.ToLower() == "true";
                                    item.TextForWeb = promo.TextForWeb;
                                    item.Quantity = TryParseDecimal(promo.Quantity);
                                    item.PromotionBarcodes = promo.AnotherBarcodes;
                                    item.IsPromotion = true;
                                    item.IsSentToEsl = false;
                                    item.OperationGuid = promo.OperationGuid;
                                }
                                itemsToUpdate.Add(item);
                                promoIdsToMark.Add(promo.Id);
                                promoCount++;
                                if (promoCount % 100 == 0)
                                {
                                    progress.Report($"Processed {promoCount} Promotions...");
                                }
                            }
                            else
                            {
                                progress.Report($"No matching Item found for Promotion ID {promo.Id} with Barcode {promo.ItemKod} and BranchId {promo.BranchId}.");
                                // Optionally log as a bad item
                            }
                        }
                        catch (Exception ex)
                        {
                            progress.Report($"Error mapping Promotion ID {promo.Id}: {ex.Message}");
                            await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "Mapping Promotion to Item", ex);
                            continue;
                        }
                    }
                    if (itemsToUpdate.Any())
                    {
                        progress.Report("Starting bulk update of Items with Promotion details...");
                        // Group by (Barcode, BranchId) if duplicate updates occur.
                        var uniqueItems = itemsToUpdate
                            .GroupBy(x => new { x.Barcode, x.BranchId })
                            .Select(g => g.OrderByDescending(item => item.CreateDateTime).First())
                            .ToList();
                        await _itemRepo.BulkInsertOrUpdateAsync(uniqueItems, bulkConfig);
                        progress.Report("Bulk update of Items with Promotions completed.");
                    }
                    if (promoIdsToMark.Any())
                    {
                        var distinctPromoIds = promoIdsToMark.Distinct().ToList();
                        progress.Report("Bulk marking Promotions as transferred...");
                        await _promoRepo.MarkAsTransferredAsync(distinctPromoIds, now);
                        progress.Report("Marked Promotions as transferred.");
                    }
                }
                else
                {
                    progress.Report("No Promotions to process.");
                }

                // ---------------------------
                // ROUND 3: Process Price Updates (new logic)
                // ---------------------------
                progress.Report("Fetching Price Updates...");
                var priceUpdates = await _priceUpdateRepository.GetNonTransferredItemsAsync();
                if (priceUpdates.Any())
                {
                    progress.Report($"Fetched {priceUpdates.Count} Price Updates...");
                    // Group price updates by (Barcode, BranchId) and pick the latest update per group.
                    var groupedPriceUpdates = priceUpdates
                        .GroupBy(p => new { p.Barcode, p.BranchId })
                        .Select(g => g.OrderByDescending(p => p.CreatedDateTime).First())
                        .ToList();

                    // Fetch matching Items for these price updates.
                    var priceUpdateKeys = groupedPriceUpdates.Select(p => (p.Barcode, p.BranchId)).Distinct().ToList();
                    var matchingItemsForPrices = await _itemRepo.GetItemsByBarcodesAndBranchIdsAsync(priceUpdateKeys);

                    // Group items by key to identify duplicates
                    var groupedItemsForPrices = matchingItemsForPrices.GroupBy(i => (i.Barcode, i.BranchId)).ToList();

                    // Log warnings for duplicates separately
                    foreach (var group in groupedItemsForPrices)
                    {
                        if (group.Count() > 1)
                        {
                             await _databaseLogger.LogServiceActionAsync($"Warning: Duplicate key ({group.Key.Barcode}, {group.Key.BranchId}) found in Items table during price update processing. Using the first occurrence.");
                             progress.Report($"Warning: Duplicate key ({group.Key.Barcode}, {group.Key.BranchId}) found in Items table. Check data integrity.");
                        }
                    }

                    // Create the dictionary using the first item from each group
                    var itemDictForPrices = groupedItemsForPrices.ToDictionary(g => g.Key, g => g.First());

                    var itemsToUpdateWithPrices = new List<Models.Models.Item>();
                    // List to keep track of price update IDs that had a matching item.
                    var matchedPuIds = new List<long>();

                    foreach (var pu in groupedPriceUpdates)
                    {
                        if (itemDictForPrices.TryGetValue((pu.Barcode, pu.BranchId), out var item))
                        {
                            UpdateItemWithPriceUpdate(item, pu);
                            itemsToUpdateWithPrices.Add(item);
                            // Only add the price update's ID if there is a matching item.
                            matchedPuIds.Add(pu.Id);
                        }
                        else
                        {
                            progress.Report($"No matching Item found for Price Update with Barcode {pu.Barcode} and BranchId {pu.BranchId}");
                        }
                    }

                    if (itemsToUpdateWithPrices.Any())
                    {
                        progress.Report("Starting bulk update of Items with Price Updates...");
                        await _itemRepo.BulkInsertOrUpdateAsync(itemsToUpdateWithPrices, bulkConfig);
                        progress.Report("Bulk update of Items with Price Updates completed.");
                    }

                    if (matchedPuIds.Any())
                    {
                        await _priceUpdateRepository.MarkAsTransferredAsync(matchedPuIds, now);
                        progress.Report("Marked Price Updates as transferred.");
                    }
                }
            
                else
                {
                    progress.Report("No Price Updates to process.");
                }

                await _databaseLogger.LogServiceActionAsync("SyncAllItemsAndPromotions completed successfully.");
                progress.Report("Synchronization completed successfully.");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "SyncAllItemsAndPromotions", ex);
                progress.Report($"Error during synchronization: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a temp AllItem to an operative Item without promotion details.
        /// (This mapping is used for the catalog upsert.)
        /// </summary>
        private Models.Models.Item MapItemWithoutPromotion(AllItem tempItem, DateTime now)
        {
            decimal? price = TryParseDecimal(tempItem.Price);
            decimal? quantity = TryParseDecimal(tempItem.Quantity);

            var item = new Models.Models.Item
            {
                CompanyId = tempItem.CompanyId,
                BranchId = tempItem.BranchId,
                Barcode = tempItem.Barcode ?? "",
                Name = tempItem.Name ?? "",
                Price = price,
                CreateDateTime = now,
                OperationGuid = tempItem.OperationGuid,
                IsSentToEsl = false,
                IsPromotion = false,
                Content = tempItem.Content,
                ContentMeasure = tempItem.ContentMeasure,
                ContentUnit = tempItem.ContentUnit,
                Size = tempItem.Size,
                SwWeighable = tempItem.SwWeighable?.ToLower() == "true",
                ManufacturingCountry = tempItem.ManufacturingCountry,
                //Quantity = quantity,
                // Initialize promotion-related fields to default values.
                PromotionKod = "",
                PromotionFromDate = null,
                PromotionToDate = null,
                TotalPromotionPrice = null,
                SwAllCustomers = false
            };
            return item;
        }
        private Models.Models.Item MapItem(AllItem tempItem, DateTime now)
        {
            decimal? price = TryParseDecimal(tempItem.Price);
            decimal? quantity = TryParseDecimal(tempItem.Quantity);

            var item = new Models.Models.Item
            {
                CompanyId = tempItem.CompanyId,
                BranchId = tempItem.BranchId,
                Barcode = tempItem.Barcode ?? "",
                Name = tempItem.Name ?? "",
                Price = price,
                CreateDateTime = now,
                OperationGuid = tempItem.OperationGuid,
                IsSentToEsl = false,
                //IsPromotion = false,
                Content = tempItem.Content,
                ContentMeasure = tempItem.ContentMeasure,
                ContentUnit = tempItem.ContentUnit,
                Size = tempItem.Size,
                SwWeighable = tempItem.SwWeighable?.ToLower() == "true",
                ManufacturingCountry = tempItem.ManufacturingCountry,
                SwPikadon = tempItem.SwPikadon?.ToLower() == "1",
                TrailingItem = tempItem.TrailingItem,
                //Quantity = quantity,
                // Initialize promotion-related fields to default values.
                //PromotionKod = "",
                //PromotionFromDate = null,
                //PromotionToDate = null,
                //TotalPromotionPrice = null,
                //SwAllCustomers = false
            };
            return item;
        }

        /// <summary>
        /// Updates an operative Item with new price details from a PriceUpdate record.
        /// </summary>
        private void UpdateItemWithPriceUpdate(Models.Models.Item item, PriceUpdate pu)
        {
            // Update price-related fields on the operative item.
            // Adjust conversion/parsing as needed; here we use TryParseDecimal.
            item.Price = TryParseDecimal(pu.Price);
            item.OperationGuid = pu.OperationGuid;
            item.IsSentToEsl = false;
            // Assume that your operative Item entity has properties for these price details.
            //item.NetPrice = TryParseDecimal(pu.NetPrice);
            //item.ShekelPrice = TryParseDecimal(pu.ShekelPrice);
            //item.ShekelNetPrice = TryParseDecimal(pu.ShekelNetPrice);
            //item.SalePrice = TryParseDecimal(pu.SalePrice);
            //// For OperationEndDate, you can update as a string (or convert to DateTime if needed)
            //// Here we simply store the provided string value.
            //item.OperationEndDate = pu.OperationEndDate;
        }

        /// <summary>
        /// Parses a string to DateTime using various formats.
        /// </summary>
        private DateTime? ParseDate(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            string[] formats = { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy", "MM/dd/yyyy HH:mm:ss", "MM/dd/yyyy" };

            if (DateTime.TryParseExact(dateString.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            return null;
        }

        /// <summary>
        /// Tries to parse a string to decimal.
        /// </summary>
        private decimal? TryParseDecimal(string? decimalString)
        {
            if (!string.IsNullOrEmpty(decimalString) && decimal.TryParse(decimalString, out var val))
            {
                return val;
            }
            return null;
        }
    }
}
