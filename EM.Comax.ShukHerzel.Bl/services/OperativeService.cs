// OperativeService.cs
using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzel.Models.DtoModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using EFCore.BulkExtensions;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        private const int BatchSize = 1000; // Adjust based on performance testing

        public OperativeService(
            IAllItemsRepository allItemsRepo,
            IPromotionsRepository promoRepo,
            IItemsRepository itemRepo,
            IDatabaseLogger databaseLogger,
            IBadItemLogRepository badItemLogRepository,
            IPriceUpdateRepository priceUpdateRepository)
        {
            _allItemsRepo = allItemsRepo;
            _promoRepo = promoRepo;
            _itemRepo = itemRepo;
            _databaseLogger = databaseLogger;
            _badItemLogRepository = badItemLogRepository;
            _priceUpdateRepository = priceUpdateRepository;
        }

        public async Task SyncAllItemsAndPromotionsAsync(IProgress<string> progress, CancellationToken cancellationToken = default)
        {
            try
            {
                await _databaseLogger.LogServiceActionAsync("Starting SyncAllItemsAndPromotions...");
                
                progress.Report("Starting synchronization of Items and Promotions...");

                // **ROUND 1: Insert all valid AllItems into the Items table**
                progress.Report("Removing duplicate operative Items...");
                await _itemRepo.CleanExpiredPromotions();
                await _allItemsRepo.RemoveDuplicateItemsAsync();

                // Fetch non-transferred AllItems
                var allItems = await _allItemsRepo.GetNonTransferredItemsAsync();
                progress.Report($"Fetched {allItems.Count} Items from temp table.");

                // Filter items with zero price
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
                    //get all item ids with zero price and mark them as bad
                    var ids = itemsWithZeroPrice.Select(x => x.Id).ToList();
                    await _allItemsRepo.MarkAsBad(ids);

                    // Remove these items from AllItems
                    allItems = allItems.Except(itemsWithZeroPrice).ToList();
                }

                // Insert valid items into the Items table
                var now = DateTime.Now; // Use UTC for consistency
                var operativeItems = allItems.Select(item => MapItemWithoutPromotion(item, now)).ToList();

                var bulkConfig = new BulkConfig
                {
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true,
                    BatchSize = Constants.OPERATIVE_BATCH_SIZE,
                    NotifyAfter = 1000,
                    UpdateByProperties = new List<string> { "Barcode", "BranchId" }
                };

                progress.Report("Starting bulk insert of valid Items...");
                await _itemRepo.BulkInsertOrUpdateAsync(operativeItems, bulkConfig);
                progress.Report("Bulk insert of valid Items completed.");
                await _databaseLogger.LogServiceActionAsync("Bulk insert of valid Items completed.");

                // Mark AllItems as transferred
                var allItemIds = allItems.Select(a => a.Id).ToList();
                await _allItemsRepo.MarkAsTransferredAsync(allItemIds, now);
                progress.Report("Marked AllItems as transferred.");
                await _databaseLogger.LogServiceActionAsync("Marked AllItems as transferred.");

                // **ROUND 2: Process promotions and update items**
                await _promoRepo.DeleteExpiredPromotionsAsync();
                var promos = await _promoRepo.GetNonTransferredPromotionsAsync();
                if (promos.Any())
                {
                    progress.Report($"Processing {promos.Count} Promotions...");

                    // Create list of unique (Barcode, BranchId) from Promotions
                    var promoKeys = promos.Select(p => (p.ItemKod, p.BranchId)).Distinct().ToList();

                    // Fetch matching Items for Promotions
                    progress.Report("Fetching matching Items for Promotions...");
                    var matchingItems = await _itemRepo.GetItemsByBarcodesAndBranchIdsAsync(promoKeys);
                    progress.Report($"Found {matchingItems.Count} matching Items for Promotions.");

                    // Create a lookup for quick access
                    var itemDict = matchingItems.ToDictionary(i => (i.Barcode, i.BranchId), i => i);

                    var itemsToUpdate = new List<Models.Models.Item>();
                    var promoIdsToMark = new List<long>();
                    var noMatchBadLogs = new List<BadItemLog>();
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
                                    item.PromotionKod =  null;
                                    item.PromotionFromDate = null;
                                    item.PromotionToDate = null;
                                    item.TotalPromotionPrice = null;
                                    item.SwAllCustomers = null;
                                    item.TextForWeb = null;
                                    item.Quantity = null;
                                    item.PromotionBarcodes = null;
                                    item.IsPromotion = false;
                                    item.IsSentToEsl = false; // Set IsTransferred to 0 (false)
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
                                    item.IsSentToEsl = false; // Set IsTransferred to 0 (false)
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
                                var message = $"No matching Item found for Promotion ID {promo.Id} with Barcode {promo.ItemKod} and BranchId {promo.BranchId}.";
                                progress.Report(message);
                                // await _databaseLogger.LogServiceActionAsync(message);
                                noMatchBadLogs.Add(new BadItemLog
                                {
                                    Barcode = promo.ItemKod ?? "",
                                    Message = "No matching Item found.",
                                    Guid = Guid.NewGuid().ToString(),
                                    TimeStamp = DateTime.Now
                                });
                                //await _badItemLogRepository.AddAsync(new BadItemLog
                                //{
                                //    Barcode = promo.ItemKod ?? "",
                                //    Message = "No matching Item found.",
                                //    Guid = Guid.NewGuid().ToString(),
                                //    TimeStamp = DateTime.UtcNow
                                //});
                            }
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = $"Error mapping Promotion ID {promo.Id}: {ex.Message}";
                            progress.Report(errorMessage);
                            await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "Mapping Promotion to Item", ex);
                            continue;
                        }
                    }

                    // Bulk update Items with Promotions
                    if (itemsToUpdate.Any())
                    {
                        progress.Report("Starting bulk update of Items with Promotions...");
                        var uniqueItems = itemsToUpdate
    .GroupBy(x => new { x.Barcode, x.BranchId })
    .Select(g =>
    {
        // If you need to combine or pick the “latest” item from the group, do it here
        // For example, keep the item with the “newest” data
        return g.OrderByDescending(item => item.CreateDateTime).First();
    })
    .ToList();

                        await _itemRepo.BulkInsertOrUpdateAsync(uniqueItems, bulkConfig);
                     //   await _itemRepo.BulkInsertOrUpdateAsync(itemsToUpdate, bulkConfig);
                        progress.Report("Bulk update of Items with Promotions completed.");
                        await _databaseLogger.LogServiceActionAsync("Bulk update of Items with Promotions completed.");
                    }
                    if (noMatchBadLogs.Any())
                    {
                        await _badItemLogRepository.BulkInsertAsync(noMatchBadLogs);
                    }

                    // Bulk mark Promotions as transferred
                    if (promoIdsToMark.Any())
                    {
                        var distinctPromoIds = promoIdsToMark.Distinct().ToList();
                        progress.Report("Bulk marking Promotions as transferred...");
                        await _promoRepo.MarkAsTransferredAsync(distinctPromoIds, now);
                        progress.Report("Marked Promotions as transferred.");
                        await _databaseLogger.LogServiceActionAsync("Marked Promotions as transferred.");
                    }
                }
                else
                {
                    progress.Report("No Promotions to process.");
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
                OperationGuid = Guid.NewGuid(),
                IsSentToEsl = false, // Assuming default value
                IsPromotion = false, // Not part of a promotion yet
                Content = tempItem.Content,
                ContentMeasure = tempItem.ContentMeasure,
                ContentUnit = tempItem.ContentUnit,
                Size = tempItem.Size,
                SwWeighable = tempItem.SwWeighable?.ToLower() == "true",
                ManufacturingCountry = tempItem.ManufacturingCountry,
                //tryparse quantity to decimal



                Quantity = quantity,

                // Initialize promotion-related fields
                PromotionKod = "",
                PromotionFromDate = null,
                PromotionToDate = null,
                TotalPromotionPrice = null,
                SwAllCustomers = false,
                
                // Serialize complex fields safely if necessary

            };
            return item;
        }

        /// <summary>
        /// Safely serializes an object to JSON, catches exceptions, and truncates to a specified max length.
        /// </summary>
        private string SafeSerializeToJson(object? data, int maxLength = 2000)
        {
            if (data == null) return string.Empty;

            try
            {
                var json = JsonConvert.SerializeObject(data);
                return json.Length > maxLength ? json.Substring(0, maxLength) : json;
            }
            catch
            {
                // Optionally log the serialization error
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses a string to DateTime.
        /// </summary>
        private DateTime? ParseDate(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            // Define the expected date formats
            string[] formats = { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy", "MM/dd/yyyy HH:mm:ss", "MM/dd/yyyy" };

            // Try parsing with the specified formats and culture
            if (DateTime.TryParseExact(
                dateString.Trim(),
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                return parsedDate;
            }

            // Optionally, log the failed parsing attempt for debugging
            // await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "ParseDate", new Exception($"Failed to parse date string: {dateString}"));

            return null;
        }

        /// <summary>
        /// Parses a string to decimal.
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
