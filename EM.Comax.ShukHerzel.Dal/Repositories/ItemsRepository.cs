using EFCore.BulkExtensions;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class ItemsRepository : BaseRepository<Item>, IItemsRepository
    {
        private readonly IDatabaseLogger _databaseLogger;
        public ItemsRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
            _databaseLogger = databaseLogger;
        }
        public async Task<Item> FindByKodAndBranchAsync(string kod, long branchId)
        {
            return await _context.Items.Where(x => x.Barcode == kod && x.BranchId == branchId).FirstOrDefaultAsync();
        }
        public async Task<List<Item>> GetItemsByBarcodesAndBranchIdsAsync(IEnumerable<(string Barcode, long BranchId)> keys)
        {
            var barcodes = keys.Select(k => k.Barcode).ToList();
            var branchIds = keys.Select(k => k.BranchId).ToList();
            return await _context.Items
                .Where(i => barcodes.Contains(i.Barcode) && branchIds.Contains(i.BranchId ?? 0)).ToListAsync();
        }
        public async Task RemoveDuplicateItemsAsync()
        {
            string sql = @"
        WITH CTE AS (
            SELECT 
                Id,
                ROW_NUMBER() OVER (PARTITION BY Barcode, BranchId ORDER BY CreateDateTime DESC) AS rn
            FROM 
                oper.items
        )
        DELETE FROM oper.items
        WHERE Id IN (SELECT Id FROM CTE WHERE rn > 1);
    ";


            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql);
                await _databaseLogger.LogServiceActionAsync("Duplicate operative Items removed successfully.");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "RemoveDuplicateItemsAsync", ex);
                throw; // Re-throw to handle upstream if necessary
            }
        }
        public async Task<IEnumerable<ItemWithBranch>> GetItemWithBranches()
        {
            // Joins Item with Branch to include StoreId
            return await _context.Items
                .Where(i => i.IsSentToEsl == false || i.IsSentToEsl == null)
                .Join(_context.Branches,
                    item => item.BranchId,
                    branch => branch.Id,
                    (item, branch) => new ItemWithBranch { Item = item, StoreId = branch.EslStoreId })
                .ToListAsync();
        }

        public async Task<List<Item>> GetItemsToSendAsync()
        {
            return await _context.Items
                .Where(i => i.IsSentToEsl == false || i.IsSentToEsl == null)
                .ToListAsync();
        }

        public async Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredDateTime)
        {
            if (ids == null || !ids.Any())
            {
                return; // No items to update
            }

            var batchSize = Constants.OPERATIVE_BATCH_SIZE; // Define an optimal batch size based on testing
            var idList = ids.ToList();

            for (int i = 0; i < idList.Count; i += batchSize)
            {
                var batchIds = idList.Skip(i).Take(batchSize).ToList();

                var itemsToUpdate = batchIds.Select(id => new Item
                {
                    Id = id,
                    IsSentToEsl = true,
                    SentToEslDate = transferredDateTime
                }).ToList();

                var bulkConfig = new BulkConfig
                {
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true,
                    BatchSize = batchSize,
                    NotifyAfter = 1000,
                    UpdateByProperties = new List<string> { "Id" },
                    PropertiesToInclude = new List<string> { "IsSentToEsl", "SentToEslDate" }
                };

                try
                {
                    await _context.BulkUpdateAsync(itemsToUpdate, bulkConfig);
                }
                catch (Exception ex)
                {
                    await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "MarkAsTransferredAsync - Batch Update", ex);
                    // Depending on requirements, decide whether to continue or halt
                    throw; // For this example, we'll re-throw
                }
            }
        }

        public async Task DeleteOldOperativeRecordsAsync(int days) // Reverted signature to match interface
        {
            const int batchSize = 1000; // Define batch size internally
            var cutoffDate = DateTime.UtcNow.AddDays(-days); // Consider using UtcNow if appropriate for your data
            int totalDeleted = 0;
            await _databaseLogger.LogServiceActionAsync($"Starting batch deletion of operative items older than {cutoffDate:yyyy-MM-dd} with batch size {batchSize}...");

            try
            {
                // 1. Find IDs of the latest record for each (Barcode, BranchId).
                // This query fetches the single latest ID for every combination.
                // Optimization: If performance is an issue on very large tables,
                // this query could potentially be scoped further.
                var latestRecordIds = await _context.Items
                    .GroupBy(i => new { i.Barcode, i.BranchId })
                    .Select(g => g.OrderByDescending(i => i.CreateDateTime).Select(i => i.Id).FirstOrDefault())
                    .ToListAsync(); // Materialize the list of latest IDs

                // Convert to HashSet for efficient lookups. Filter out default(long) if Id is non-nullable.
                var latestRecordIdSet = new HashSet<long>(latestRecordIds.Where(id => id != 0));
                await _databaseLogger.LogServiceActionAsync($"Identified {latestRecordIdSet.Count} unique latest operative items to preserve.");

                while (true)
                {
                    // 2. Find a batch of IDs to delete: older than cutoff, IsSentToEsl=1, and NOT in the latest set
                    var idsToDelete = await _context.Items
                        .Where(i => i.IsSentToEsl == true
                                    && i.CreateDateTime < cutoffDate
                                    && !latestRecordIdSet.Contains(i.Id)) // Exclude the latest ones
                        .Select(i => i.Id) // Select only the IDs
                        .Take(batchSize)   // Take only a batch
                        .ToListAsync();    // Materialize the batch of IDs

                    if (!idsToDelete.Any())
                    {
                        await _databaseLogger.LogServiceActionAsync("No more old operative items found to delete in this pass.");
                        break; // Exit the loop if no records are found to delete
                    }

                    // 3. Delete the batch using ExecuteDeleteAsync (Requires EF Core 7+)
                    int deletedInBatch = 0;
                    try
                    {
                         deletedInBatch = await _context.Items
                                                .Where(i => idsToDelete.Contains(i.Id))
                                                .ExecuteDeleteAsync(); // Perform the batch delete
                    }
                    catch(Exception batchEx)
                    {
                         await _databaseLogger.LogErrorAsync("ITEMS_REPOSITORY", $"Error deleting batch of {idsToDelete.Count} items. IDs: {string.Join(",", idsToDelete)}", batchEx);
                         // Depending on requirements, you might want to break or continue here.
                         // For now, we re-throw to halt the process on batch failure.
                         throw;
                    }


                    totalDeleted += deletedInBatch;
                    await _databaseLogger.LogServiceActionAsync($"Deleted batch of {deletedInBatch} old operative items. Total deleted so far: {totalDeleted}.");

                    // If we deleted fewer records than the batch size, it implies we might be done.
                    // Or if ExecuteDeleteAsync returns 0 unexpectedly.
                    if (deletedInBatch == 0 || deletedInBatch < batchSize)
                    {
                        break;
                    }

                    // Optional: Add a small delay to prevent overwhelming the DB server
                    // await Task.Delay(100);
                }

                await _databaseLogger.LogServiceActionAsync($"Finished batch deletion. Total old operative Items removed: {totalDeleted}.");
            }
            catch (Exception ex)
            {
                // Log the main exception if it occurs outside the batch loop or is re-thrown
                await _databaseLogger.LogErrorAsync("ITEMS_REPOSITORY", "Error during DeleteOldOperativeRecordsAsync", ex);
                throw; // Re-throw to allow upstream handling
            }
        }


        public async Task CleanExpiredPromotions()
        {
            // Use DateTime.Today to disregard the hour
            var today = DateTime.Today;

            // Only select items where the promotion expiration date is before today.
            var expiredPromotions = await _context.Items
                .Where(x => x.IsPromotion == true && x.PromotionToDate < today)
                .ToListAsync();
            // all promotions related fields set to null
            foreach (var item in expiredPromotions)
            {
                item.IsPromotion = false;
                item.PromotionFromDate = null;
                item.PromotionToDate = null;
                item.PromotionKod = null;
                item.TotalPromotionPrice = null;
                item.IsSentToEsl = false;
            }
            await _context.BulkUpdateAsync(expiredPromotions);
        }

        public async Task<List<Item>> SearchItemsAsync(string barcode = null, long? branchId = null, string name = null)
        {
            var query = _context.Items.AsQueryable();
            
            if (!string.IsNullOrEmpty(barcode))
                query = query.Where(i => i.Barcode.Contains(barcode));
                
            if (branchId.HasValue)
                query = query.Where(i => i.BranchId == branchId.Value);
                
            if (!string.IsNullOrEmpty(name))
                query = query.Where(i => i.Name.Contains(name));
                
            return await query.ToListAsync();
        }

        public async Task SetItemNotSentAsync(long itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.IsSentToEsl = false;
                await _context.SaveChangesAsync();
                await _databaseLogger.LogServiceActionAsync($"Item ID {itemId} marked as not sent to ESL.");
            }
        }

        public async Task RemovePromotionAsync(long itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.IsPromotion = false;
                item.PromotionKod = null;
                item.PromotionFromDate = null;
                item.PromotionToDate = null;
                item.TotalPromotionPrice = null;
                item.SwAllCustomers = null;
                item.TextForWeb = null;
                item.Quantity = null;
                item.PromotionBarcodes = null;
                item.IsSentToEsl = false;
                await _context.SaveChangesAsync();
                await _databaseLogger.LogServiceActionAsync($"Promotion removed from Item ID {itemId} and marked for resending.");
            }
        }
    }
}
