using EFCore.BulkExtensions;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class PriceUpdateRepository : BaseRepository<PriceUpdate>, IPriceUpdateRepository
    {
        private readonly IDatabaseLogger _databaseLogger; // Added logger field

        // Updated constructor to inject IDatabaseLogger
        public PriceUpdateRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
             _databaseLogger = databaseLogger; // Store logger instance
        }

        public async Task DeleteTransferredItemsOlderThanAsync(int days)
        {
            const int batchSize = 1000; // Define batch size internally
            var cutoffDate = DateTime.UtcNow.AddDays(-days); // Use UtcNow for consistency
            int totalDeleted = 0;
            await _databaseLogger.LogServiceActionAsync($"Starting batch deletion of PriceUpdate entries older than {cutoffDate:yyyy-MM-dd} with batch size {batchSize}...");

            try
            {
                while (true)
                {
                    // Find a batch of IDs to delete based on CreatedDateTime
                    var idsToDelete = await _context.PriceUpdates
                        .Where(pu => pu.CreatedDateTime < cutoffDate)
                        .Select(pu => pu.Id) // Select only the IDs
                        .Take(batchSize)     // Take only a batch
                        .ToListAsync();      // Materialize the batch of IDs

                    if (!idsToDelete.Any())
                    {
                        await _databaseLogger.LogServiceActionAsync("No more old PriceUpdate entries found to delete in this pass.");
                        break; // Exit the loop if no records are found
                    }

                    // Delete the batch using ExecuteDeleteAsync (Requires EF Core 7+)
                    int deletedInBatch = 0;
                    try
                    {
                        deletedInBatch = await _context.PriceUpdates
                                               .Where(pu => idsToDelete.Contains(pu.Id))
                                               .ExecuteDeleteAsync(); // Perform the batch delete
                    }
                    catch (Exception batchEx)
                    {
                        await _databaseLogger.LogErrorAsync("PRICEUPDATE_REPOSITORY", $"Error deleting batch of {idsToDelete.Count} PriceUpdate entries. IDs: {string.Join(",", idsToDelete)}", batchEx);
                        throw; // Re-throw to halt the process on batch failure
                    }

                    totalDeleted += deletedInBatch;
                    await _databaseLogger.LogServiceActionAsync($"Deleted batch of {deletedInBatch} old PriceUpdate entries. Total deleted so far: {totalDeleted}.");

                    // If we deleted fewer records than the batch size, it implies we might be done.
                    if (deletedInBatch == 0 || deletedInBatch < batchSize)
                    {
                        break;
                    }

                    // Optional: await Task.Delay(100);
                }

                await _databaseLogger.LogServiceActionAsync($"Finished batch deletion. Total old PriceUpdate entries removed: {totalDeleted}.");
            }
            catch (Exception ex)
            {
                // Log the main exception
                await _databaseLogger.LogErrorAsync("PRICEUPDATE_REPOSITORY", "Error during DeleteTransferredItemsOlderThanAsync", ex);
                throw; // Re-throw
            }
        }

        public async Task<List<PriceUpdate>> GetNonTransferredItemsAsync()
        {
            //get all marked as not transferred
            return await _context.PriceUpdates.Where(x => x.IsTransferredToOper == false).ToListAsync();
        }

        public async Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime)
        {
            //mark all ids istransferred to oper and update transfer date
            var items = await _context.PriceUpdates.Where(x => ids.Contains(x.Id)).ToListAsync();
            items.ForEach(x =>
            {
                x.IsTransferredToOper = true;
                x.TransferredDateTime = transferredTime;
            });
            await _context.BulkUpdateAsync(items);
        }

        public async Task RemoveDuplicateItemsAsync()
        {
            string sql = @"
        WITH CTE AS (
            SELECT 
                Id,
                ROW_NUMBER() OVER (PARTITION BY Barcode, BranchId ORDER BY CreatedDateTime DESC) AS rn
            FROM 
                temp.PriceUpdates
            WHERE 
                IsTransferredToOper = 0
        )
        DELETE FROM temp.PriceUpdates
        WHERE Id IN (SELECT Id FROM CTE WHERE rn > 1);
    ";


            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql);
              //  await _databaseLogger.LogServiceActionAsync("Duplicate operative AllItems removed successfully.");
            }
            catch (Exception ex)
            {
               // await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "RemoveDuplicateItemsAsync", ex);
              //  throw; // Re-throw to handle upstream if necessary
            }
        }
    }
}
