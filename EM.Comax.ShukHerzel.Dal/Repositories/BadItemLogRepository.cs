// BadItemLogRepository.cs
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System; // Added for DateTime
using System.Collections.Generic;
using System.Linq; // Added for LINQ methods
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class BadItemLogRepository : BaseRepository<BadItemLog>, IBadItemLogRepository
    {
        private readonly IDatabaseLogger _databaseLogger; // Added logger field

        // Updated constructor to inject IDatabaseLogger
        public BadItemLogRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
            _databaseLogger = databaseLogger; // Store logger instance
        }

        public async Task BulkInsertBadItemsAsync(IEnumerable<BadItemLog> badItems, int batchSize = 1000)
        {
            await _context.BulkInsertAsync(badItems, new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = true,
                BatchSize = batchSize
            });
        }

        // Refactored method to use batch deletion
        public async Task DeleteLogsOlderThanAsync(int days)
        {
            const int batchSize = 1000; // Define batch size internally
            var cutoffDate = DateTime.UtcNow.AddDays(-days); // Use UtcNow for consistency if TimeStamp is UTC
            int totalDeleted = 0;
            await _databaseLogger.LogServiceActionAsync($"Starting batch deletion of BadItemLog entries older than {cutoffDate:yyyy-MM-dd} with batch size {batchSize}...");

            try
            {
                while (true)
                {
                    // Find a batch of IDs to delete
                    var idsToDelete = await _context.BadItemLogs
                        .Where(log => log.TimeStamp < cutoffDate)
                        .Select(log => log.Id) // Select only the IDs
                        .Take(batchSize)      // Take only a batch
                        .ToListAsync();       // Materialize the batch of IDs

                    if (!idsToDelete.Any())
                    {
                        await _databaseLogger.LogServiceActionAsync("No more old BadItemLog entries found to delete in this pass.");
                        break; // Exit the loop if no records are found
                    }

                    // Delete the batch using ExecuteDeleteAsync (Requires EF Core 7+)
                    int deletedInBatch = 0;
                    try
                    {
                        deletedInBatch = await _context.BadItemLogs
                                               .Where(log => idsToDelete.Contains(log.Id))
                                               .ExecuteDeleteAsync(); // Perform the batch delete
                    }
                    catch (Exception batchEx)
                    {
                        await _databaseLogger.LogErrorAsync("BADITEMLOG_REPOSITORY", $"Error deleting batch of {idsToDelete.Count} BadItemLog entries. IDs: {string.Join(",", idsToDelete)}", batchEx);
                        throw; // Re-throw to halt the process on batch failure
                    }

                    totalDeleted += deletedInBatch;
                    await _databaseLogger.LogServiceActionAsync($"Deleted batch of {deletedInBatch} old BadItemLog entries. Total deleted so far: {totalDeleted}.");

                    // If we deleted fewer records than the batch size, it implies we might be done.
                    if (deletedInBatch == 0 || deletedInBatch < batchSize)
                    {
                        break;
                    }

                    // Optional: await Task.Delay(100);
                }

                await _databaseLogger.LogServiceActionAsync($"Finished batch deletion. Total old BadItemLog entries removed: {totalDeleted}.");
            }
            catch (Exception ex)
            {
                // Log the main exception
                await _databaseLogger.LogErrorAsync("BADITEMLOG_REPOSITORY", "Error during DeleteLogsOlderThanAsync", ex);
                throw; // Re-throw
            }
        }
    }
}
