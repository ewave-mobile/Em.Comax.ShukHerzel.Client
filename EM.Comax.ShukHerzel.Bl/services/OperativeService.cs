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
        "XmlId",
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
                progress.Report($"Found {promos.Count} active promotions (future promotions excluded until start date).");
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
                    
                    // Group promotions by (ItemKod, BranchId) to handle multiple promotions for same item
                    var groupedPromos = promos.GroupBy(p => new { p.ItemKod, p.BranchId }).ToList();
                    progress.Report($"Found {groupedPromos.Count} unique item-branch combinations for promotions...");
                    
                    foreach (var promoGroup in groupedPromos)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        try
                        {
                            // Select the promotion with highest rating, then lowest price as tiebreaker
                            var selectedPromo = promoGroup
                                .OrderByDescending(p => 
                                {
                                    if (decimal.TryParse(p.Rating, out var rating))
                                        return rating;
                                    return 0m;
                                })
                                .ThenBy(p => 
                                {
                                    if (decimal.TryParse(p.Total, out var total))
                                        return total;
                                    return decimal.MaxValue; // If can't parse, put at end
                                })
                                .First();
                            
                            // Mark all other promotions in this group as transferred since we're only using the selected one
                            var otherPromosInGroup = promoGroup.Where(p => p.Id != selectedPromo.Id).ToList();
                            if (otherPromosInGroup.Any())
                            {
                                progress.Report($"Skipping {otherPromosInGroup.Count} lower-rated/higher-priced promotions for ItemKod: {selectedPromo.ItemKod}, BranchId: {selectedPromo.BranchId}");
                                promoIdsToMark.AddRange(otherPromosInGroup.Select(p => p.Id));
                            }
                            
                            progress.Report($"Selected promotion for ItemKod: {selectedPromo.ItemKod}, BranchId: {selectedPromo.BranchId} - Rating: {selectedPromo.Rating ?? "0"}, Price: {selectedPromo.Total ?? "N/A"}");
                            
                            var key = (selectedPromo.ItemKod, selectedPromo.BranchId);
                            Models.Models.Item item = null;
                            
                            // First try barcode lookup
                            if (itemDict.TryGetValue(key, out item))
                            {
                                // Found by barcode - proceed with existing logic
                            }
                            else
                            {
                                // Fallback: try finding by XmlId if barcode didn't match
                                item = matchingItems.FirstOrDefault(i => 
                                    i.XmlId == selectedPromo.ItemKod && i.BranchId == selectedPromo.BranchId);
                            }
                            
                            if (item != null)
                            {
                                if (selectedPromo.SwActive?.ToLower() != "true")
                                {
                                    // Only remove promotion details if the item has the same promotion code
                                    if (item.PromotionKod == selectedPromo.Kod)
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
                                        item.TotalForActivate = null;
                                        item.PromotionQuantity = null;
                                        item.GetDiscountTotal = null;
                                        item.GetCmt = null;
                                        item.GetDiscountPrecent = null;
                                        item.GetTotal = null;
                                        item.PromotionMinQty = null;
                                        item.PromotionMaxQty = null;
                                        item.Rating = null;
                                        progress.Report($"Cancelled promotion {selectedPromo.Kod} for ItemKod: {selectedPromo.ItemKod}");
                                    }
                                    else
                                    {
                                        progress.Report($"Skipping cancellation for ItemKod: {selectedPromo.ItemKod} - item has different promotion code ({item.PromotionKod}) than the one being cancelled ({selectedPromo.Kod})");
                                    }
                                }
                                else
                                {
                                    // Check if item already has a promotion and compare ratings/prices
                                    bool shouldUpdatePromotion = true;
                                    var today = DateTime.Today; // Date-only comparison, includes current day
                                    
                                    if (item.IsPromotion == true && !string.IsNullOrEmpty(item.Rating))
                                    {
                                        // First check if existing promotion is still valid (date validation)
                                        bool existingPromotionValid = true;
                                        
                                        if (item.PromotionFromDate.HasValue && item.PromotionFromDate.Value.Date > today)
                                        {
                                            existingPromotionValid = false;
                                            progress.Report($"Existing promotion for ItemKod: {selectedPromo.ItemKod} not yet started (starts: {item.PromotionFromDate.Value.Date:dd/MM/yyyy})");
                                        }
                                        else if (item.PromotionToDate.HasValue && item.PromotionToDate.Value.Date < today)
                                        {
                                            existingPromotionValid = false;
                                            progress.Report($"Existing promotion for ItemKod: {selectedPromo.ItemKod} has expired (ended: {item.PromotionToDate.Value.Date:dd/MM/yyyy})");
                                        }
                                        
                                        if (existingPromotionValid)
                                        {
                                            // Item already has a valid promotion, compare with the new one
                                            decimal existingRating = decimal.TryParse(item.Rating, out var eRating) ? eRating : 0m;
                                            decimal newRating = decimal.TryParse(selectedPromo.Rating, out var nRating) ? nRating : 0m;
                                            
                                            if (newRating < existingRating)
                                            {
                                                // New promotion has lower rating, keep existing
                                                shouldUpdatePromotion = false;
                                                progress.Report($"Keeping existing promotion for ItemKod: {selectedPromo.ItemKod} (existing rating: {existingRating} > new rating: {newRating})");
                                            }
                                            else if (newRating == existingRating)
                                            {
                                                // Same rating, compare prices
                                                decimal existingPrice = item.TotalPromotionPrice ?? decimal.MaxValue;
                                                decimal newPrice = TryParseDecimal(selectedPromo.Total) ?? decimal.MaxValue;
                                                
                                                if (newPrice >= existingPrice)
                                                {
                                                    // New promotion has higher or equal price, keep existing
                                                    shouldUpdatePromotion = false;
                                                    progress.Report($"Keeping existing promotion for ItemKod: {selectedPromo.ItemKod} (same rating: {existingRating}, existing price: {existingPrice} <= new price: {newPrice})");
                                                }
                                                else
                                                {
                                                    progress.Report($"Replacing existing promotion for ItemKod: {selectedPromo.ItemKod} (same rating: {existingRating}, new price: {newPrice} < existing price: {existingPrice})");
                                                }
                                            }
                                            else
                                            {
                                                progress.Report($"Replacing existing promotion for ItemKod: {selectedPromo.ItemKod} (new rating: {newRating} > existing rating: {existingRating})");
                                            }
                                        }
                                        else
                                        {
                                            // Existing promotion is expired/invalid, update with new promotion
                                            progress.Report($"Replacing expired/invalid promotion for ItemKod: {selectedPromo.ItemKod} with new promotion");
                                        }
                                    }
                                    
                                    // Also validate the new promotion dates before applying
                                    if (shouldUpdatePromotion)
                                    {
                                        // Check if the new promotion is currently valid (date validation)
                                        var newPromotionFromDate = ParseDate(selectedPromo.FromDate);
                                        var newPromotionToDate = ParseDate(selectedPromo.ToDate);
                                        
                                        bool newPromotionValid = true;
                                        if (newPromotionFromDate.HasValue && newPromotionFromDate.Value.Date > today)
                                        {
                                            newPromotionValid = false;
                                            progress.Report($"New promotion for ItemKod: {selectedPromo.ItemKod} not yet started (starts: {newPromotionFromDate.Value.Date:dd/MM/yyyy}), skipping");
                                        }
                                        else if (newPromotionToDate.HasValue && newPromotionToDate.Value.Date < today)
                                        {
                                            newPromotionValid = false;
                                            progress.Report($"New promotion for ItemKod: {selectedPromo.ItemKod} has expired (ended: {newPromotionToDate.Value.Date:dd/MM/yyyy}), skipping");
                                        }
                                        
                                        if (!newPromotionValid)
                                        {
                                            shouldUpdatePromotion = false;
                                        }
                                    }
                                    
                                    if (shouldUpdatePromotion)
                                    {
                                        // Update Item with Promotion details
                                        item.PromotionKod = selectedPromo.Kod ?? "";
                                        item.PromotionFromDate = ParseDate(selectedPromo.FromDate);
                                        item.PromotionToDate = ParseDate(selectedPromo.ToDate);
                                        item.TotalPromotionPrice = TryParseDecimal(selectedPromo.Total);
                                        item.SwAllCustomers = selectedPromo.SwAllCustomers?.ToLower() == "true";
                                        item.TextForWeb = selectedPromo.TextForWeb;
                                        item.Quantity = TryParseDecimal(selectedPromo.Quantity);
                                        item.PromotionBarcodes = selectedPromo.AnotherBarcodes;
                                        item.IsPromotion = true;
                                        item.IsSentToEsl = false;
                                        item.OperationGuid = selectedPromo.OperationGuid;
                                        item.TotalForActivate = selectedPromo.TotalForActivate;
                                        item.PromotionQuantity = selectedPromo.Quantity;
                                        item.GetDiscountTotal = selectedPromo.GetDiscountTotal;
                                        item.GetCmt = selectedPromo.GetCmt;
                                        item.GetDiscountPrecent = selectedPromo.GetDiscountPrecent;
                                        item.GetTotal = selectedPromo.GetTotal;
                                        item.PromotionMinQty = selectedPromo.MinQty;
                                        item.PromotionMaxQty = selectedPromo.MaxQty;
                                        item.Rating = selectedPromo.Rating;
                                    }
                                }
                                itemsToUpdate.Add(item);
                                promoIdsToMark.Add(selectedPromo.Id);
                                promoCount++;
                                if (promoCount % 100 == 0)
                                {
                                    progress.Report($"Processed {promoCount} Promotions...");
                                }
                            }
                            else
                            {
                                progress.Report($"No matching Item found for Promotion ID {selectedPromo.Id} with ItemKod/XmlId {selectedPromo.ItemKod} and BranchId {selectedPromo.BranchId}.");
                                // Optionally log as a bad item
                            }
                        }
                        catch (Exception ex)
                        {
                            progress.Report($"Error mapping Promotion group for ItemKod {promoGroup.Key.ItemKod}: {ex.Message}");
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
                XmlId = tempItem.XmlId ?? "",
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

        /// <summary>
        /// Searches for items in the operative table based on the provided criteria
        /// </summary>
        public async Task<List<Models.Models.Item>> SearchItemsAsync(string barcode = null, long? branchId = null, string name = null)
        {
            try
            {
                await _databaseLogger.LogServiceActionAsync($"Searching for items with barcode: {barcode ?? "any"}, branchId: {branchId?.ToString() ?? "any"}, name: {name ?? "any"}");
                return await _itemRepo.SearchItemsAsync(barcode, branchId, name);
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "SearchItemsAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Sets an item's IsSentToEsl flag to false so it will be sent in the next service iteration
        /// </summary>
        public async Task SetItemNotSentAsync(long itemId)
        {
            try
            {
                await _databaseLogger.LogServiceActionAsync($"Setting item ID {itemId} as not sent to ESL");
                await _itemRepo.SetItemNotSentAsync(itemId);
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "SetItemNotSentAsync", ex);
                throw;
            }
        }

        /// <summary>
        /// Removes promotion details from an item and sets it to be resent
        /// </summary>
        public async Task RemovePromotionAsync(long itemId)
        {
            try
            {
                await _databaseLogger.LogServiceActionAsync($"Removing promotion from item ID {itemId}");
                await _itemRepo.RemovePromotionAsync(itemId);
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "RemovePromotionAsync", ex);
                throw;
            }
        }
    }
}
