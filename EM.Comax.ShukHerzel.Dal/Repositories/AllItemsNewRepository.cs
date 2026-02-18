using EFCore.BulkExtensions;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants = EM.Comax.ShukHerzl.Common.Constants;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class AllItemsNewRepository : BaseRepository<AllItemComax>, IAllItemsNewRepository
    {
        private readonly IDatabaseLogger _databaseLogger;
        public AllItemsNewRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
            _databaseLogger = databaseLogger;
        }

        public async Task<List<AllItemComax>> GetNonTransferredItemsAsync()
        {
            //get all non transferred items
            return await _context.AllItemComax.Where(x => x.IsTransferredToOper == false && x.IsBad == false).ToListAsync();
        }
        public async Task RemoveDuplicateItemsAsync()
        {
            string sql = @"
        WITH CTE AS (
            SELECT 
                Id,
                ROW_NUMBER() OVER (PARTITION BY Barcode, BranchId ORDER BY CreatedDateTime DESC) AS rn
            FROM 
                temp.AllItemsComax
            WHERE 
                IsTransferredToOper = 0
        )
        DELETE FROM temp.AllItemsComax
        WHERE Id IN (SELECT Id FROM CTE WHERE rn > 1);
    ";


            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql);
                await _databaseLogger.LogServiceActionAsync("Duplicate operative AllItemComax removed successfully.");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "RemoveDuplicateItemsAsync", ex);
                throw; // Re-throw to handle upstream if necessary
            }
        }


        public async Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime)
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

                var itemsToUpdate = batchIds.Select(id => new AllItemComax
                {
                    Id = id,
                    IsTransferredToOper = true,
                    TransferredDateTime = transferredTime
                }).ToList();

                var bulkConfig = new BulkConfig
                {
                    PreserveInsertOrder = true,
 
                    BatchSize = batchSize,
                    NotifyAfter = 1000,
                    UpdateByProperties = new List<string> { "Id" },
                    PropertiesToInclude = new List<string> { "IsTransferredToOper", "TransferredDateTime" }
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

        public async Task DeleteTransferredItemsOlderThanAsync(int days)
        {
            const int batchSize = 1000; // Define batch size internally
            var cutoffDate = DateTime.UtcNow.AddDays(-days); // Use UtcNow for consistency
            int totalDeleted = 0;
            await _databaseLogger.LogServiceActionAsync($"Starting batch deletion of AllItemComax entries older than {cutoffDate:yyyy-MM-dd} with batch size {batchSize}...");

            try
            {
                while (true)
                {
                    // Find a batch of IDs to delete:
                    // Items that ARE transferred AND their transfer date is old
                    // OR items (regardless of transfer status) whose creation date is old
                    var idsToDelete = await _context.AllItemComax
                        .Where(item => (item.IsTransferredToOper == true && item.TransferredDateTime < cutoffDate)
                                    || item.CreatedDateTime < cutoffDate)
                        .Select(item => item.Id) // Select only the IDs
                        .Take(batchSize)         // Take only a batch
                        .ToListAsync();          // Materialize the batch of IDs

                    if (!idsToDelete.Any())
                    {
                        await _databaseLogger.LogServiceActionAsync("No more old AllItemComax entries found to delete in this pass.");
                        break; // Exit the loop if no records are found
                    }

                    // Delete the batch using ExecuteDeleteAsync (Requires EF Core 7+)
                    int deletedInBatch = 0;
                    try
                    {
                        deletedInBatch = await _context.AllItemComax
                                               .Where(item => idsToDelete.Contains(item.Id))
                                               .ExecuteDeleteAsync(); // Perform the batch delete
                    }
                    catch (Exception batchEx)
                    {
                        await _databaseLogger.LogErrorAsync("ALLITEMS_REPOSITORY", $"Error deleting batch of {idsToDelete.Count} AllItemComax entries. IDs: {string.Join(",", idsToDelete)}", batchEx);
                        throw; // Re-throw to halt the process on batch failure
                    }

                    totalDeleted += deletedInBatch;
                    await _databaseLogger.LogServiceActionAsync($"Deleted batch of {deletedInBatch} old AllItemComax entries. Total deleted so far: {totalDeleted}.");

                    // If we deleted fewer records than the batch size, it implies we might be done.
                    if (deletedInBatch == 0 || deletedInBatch < batchSize)
                    {
                        break;
                    }

                    // Optional: await Task.Delay(100);
                }

                await _databaseLogger.LogServiceActionAsync($"Finished batch deletion. Total old AllItemComax entries removed: {totalDeleted}.");
            }
            catch (Exception ex)
            {
                // Log the main exception
                await _databaseLogger.LogErrorAsync("ALLITEMS_REPOSITORY", "Error during DeleteTransferredItemsOlderThanAsync", ex);
                throw; // Re-throw
            }
        }

        public async Task MarkAsBad(IEnumerable<long> ids)
        {
            //bulk update items as bad
            if (ids == null || !ids.Any())
            {
                return; // No items to update
            }

            var batchSize = Constants.OPERATIVE_BATCH_SIZE; // Define an optimal batch size based on testing
            var idList = ids.ToList();

            for (int i = 0; i < idList.Count; i += batchSize)
            {
                var batchIds = idList.Skip(i).Take(batchSize).ToList();

                var itemsToUpdate = batchIds.Select(id => new AllItemComax
                {
                    Id = id,
                    IsBad = true

                }).ToList();

                var bulkConfig = new BulkConfig
                {
                    PreserveInsertOrder = true,
                    SetOutputIdentity = true,
                    BatchSize = batchSize,
                    NotifyAfter = 1000,
                    UpdateByProperties = new List<string> { "Id" },
                    PropertiesToInclude = new List<string> { "IsBad" }
                };

                try
                {
                    await _context.BulkUpdateAsync(itemsToUpdate, bulkConfig);
                }
                catch (Exception ex)
                {
                    await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "MarkAsBad - Batch Update", ex);
                    // Depending on requirements, decide whether to continue or halt
                    throw; // For this example, we'll re-throw
                }
            }
        }
    }
}
