using EFCore.BulkExtensions;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class PromotionsRepository : BaseRepository<Promotion>, IPromotionsRepository
    {
        private readonly IDatabaseLogger _databaseLogger; // Added logger field

        // Updated constructor to inject IDatabaseLogger
        public PromotionsRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
             _databaseLogger = databaseLogger; // Store logger instance
        }

        public async Task DeleteExpiredPromotionsAsync()
{
    // Get today's date (time set to midnight)
    var today = DateTime.Today;

    // Filter promotions where ToDate is not null or whitespace
    var promotions = await _context.Promotions
        .Where(x => !string.IsNullOrWhiteSpace(x.ToDate))
        .ToListAsync();

    // Parse and filter the expired promotions by comparing only the date portion
    var expiredPromotionsToDelete = promotions
        .Where(promotion =>
        {
            if (DateTime.TryParseExact(
                    promotion.ToDate.Trim(),
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                // Compare only the date parts: if parsed date is before today, mark for deletion
                return parsedDate.Date < today;
            }
            return false;
        })
        .ToList();

    // Bulk delete expired promotions if any exist
    if (expiredPromotionsToDelete.Any())
    {
        await _context.BulkDeleteAsync(expiredPromotionsToDelete);
    }
}


        public async Task DeleteTransferredItemsOlderThanAsync(int retentionDays)
        {
            const int batchSize = 1000; // Define batch size internally
            var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays); // Use UtcNow for consistency
            int totalDeleted = 0;
            await _databaseLogger.LogServiceActionAsync($"Starting batch deletion of Promotion entries older than {cutoffDate:yyyy-MM-dd} with batch size {batchSize}...");

            try
            {
                while (true)
                {
                    // Find a batch of IDs to delete:
                    // Promotions that ARE transferred AND their transfer date is old
                    // OR promotions (regardless of transfer status) whose creation date is old
                    var idsToDelete = await _context.Promotions
                        .Where(promo => (promo.IsTransferredToOper == true && promo.TransferredDateTime < cutoffDate)
                                     || promo.CreatedDateTime < cutoffDate)
                        .Select(promo => promo.Id) // Select only the IDs
                        .Take(batchSize)          // Take only a batch
                        .ToListAsync();           // Materialize the batch of IDs

                    if (!idsToDelete.Any())
                    {
                        await _databaseLogger.LogServiceActionAsync("No more old Promotion entries found to delete in this pass.");
                        break; // Exit the loop if no records are found
                    }

                    // Delete the batch using ExecuteDeleteAsync (Requires EF Core 7+)
                    int deletedInBatch = 0;
                    try
                    {
                        deletedInBatch = await _context.Promotions
                                               .Where(promo => idsToDelete.Contains(promo.Id))
                                               .ExecuteDeleteAsync(); // Perform the batch delete
                    }
                    catch (Exception batchEx)
                    {
                        await _databaseLogger.LogErrorAsync("PROMOTIONS_REPOSITORY", $"Error deleting batch of {idsToDelete.Count} Promotion entries. IDs: {string.Join(",", idsToDelete)}", batchEx);
                        throw; // Re-throw to halt the process on batch failure
                    }

                    totalDeleted += deletedInBatch;
                    await _databaseLogger.LogServiceActionAsync($"Deleted batch of {deletedInBatch} old Promotion entries. Total deleted so far: {totalDeleted}.");

                    // If we deleted fewer records than the batch size, it implies we might be done.
                    if (deletedInBatch == 0 || deletedInBatch < batchSize)
                    {
                        break;
                    }

                    // Optional: await Task.Delay(100);
                }

                await _databaseLogger.LogServiceActionAsync($"Finished batch deletion. Total old Promotion entries removed: {totalDeleted}.");
            }
            catch (Exception ex)
            {
                // Log the main exception
                await _databaseLogger.LogErrorAsync("PROMOTIONS_REPOSITORY", "Error during DeleteTransferredItemsOlderThanAsync", ex);
                throw; // Re-throw
            }
        }

        public async Task<List<Promotion>> GetNonTransferredPromotionsAsync()
        {
            return await _context.Promotions.Where(x => x.IsTransferredToOper == false).ToListAsync();
        }

        public async Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime)
        {
            //mark promotions as transferred
            var promotions = _context.Promotions.Where(x => ids.Contains(x.Id)).ToList();
            foreach (var promotion in promotions)
            {
                promotion.IsTransferredToOper = true;
                promotion.TransferredDateTime = transferredTime;
            }

            var bulkConfig = new BulkConfig
            {
                PreserveInsertOrder = true,
                NotifyAfter = 1000,
                UpdateByProperties = new List<string> { "Id" },
                PropertiesToInclude = new List<string> { "IsTransferredToOper", "TransferredDateTime" }
            };
            await _context.BulkUpdateAsync(promotions, bulkConfig);
        }

        public async Task<List<Promotion>> SearchPromotionsAsync(string kod = null, string itemKod = null, long? branchId = null)
        {
            var query = _context.Promotions.AsQueryable();
            
            if (!string.IsNullOrEmpty(kod))
                query = query.Where(p => p.Kod.Contains(kod));
                
            if (!string.IsNullOrEmpty(itemKod))
                query = query.Where(p => p.ItemKod.Contains(itemKod));
                
            if (branchId.HasValue)
                query = query.Where(p => p.BranchId == branchId.Value);
                
            return await query.ToListAsync();
        }

        public async Task SetPromotionNotTransferredAsync(long promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion != null)
            {
                promotion.IsTransferredToOper = false;
                promotion.TransferredDateTime = null;
                await _context.SaveChangesAsync();
                await _databaseLogger.LogServiceActionAsync($"Promotion ID {promotionId} marked as not transferred to operative table.");
            }
        }
    }
}
