using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Needed for ExecuteUpdateAsync

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        private readonly IDatabaseLogger _databaseLogger; // Added logger

        // Inject IDatabaseLogger
        public BranchRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
            _databaseLogger = databaseLogger; // Store logger
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesByCompanyIdAsync(long companyId)
        {
            // Consider removing Task.Run if the underlying operation is already async
            // return await _context.Branches.Where(b => b.CompanyId == companyId).ToListAsync();
             return await Task.Run(() => _context.Branches.Where(b => b.CompanyId == companyId).ToList());
        }

        // --- Implementation of specific timestamp update methods ---

        public async Task UpdateLastCatalogTimestampAsync(long branchId, DateTime timestamp)
        {
            try
            {
                int affectedRows = await _context.Branches
                    .Where(b => b.Id == branchId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(b => b.LastCatalogTimeStamp, timestamp));

                if (affectedRows == 0)
                {
                    // Log as a service action instead of a warning
                    await _databaseLogger.LogServiceActionAsync($"Warning: Attempted to update LastCatalogTimeStamp for non-existent Branch ID: {branchId}");
                }
                 // Optional: Log success if needed, but might be too verbose
                 // await _databaseLogger.LogServiceActionAsync($"Updated LastCatalogTimeStamp for Branch ID: {branchId} to {timestamp:O}");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("BRANCH_REPOSITORY", $"Error updating LastCatalogTimeStamp for Branch ID: {branchId}", ex);
                throw; // Re-throw to allow service layer to handle
            }
        }

        public async Task UpdateLastPromotionTimestampAsync(long branchId, DateTime timestamp)
        {
             try
            {
                int affectedRows = await _context.Branches
                    .Where(b => b.Id == branchId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(b => b.LastPromotionTimeStamp, timestamp));

                if (affectedRows == 0)
                {
                     // Log as a service action instead of a warning
                     await _databaseLogger.LogServiceActionAsync($"Warning: Attempted to update LastPromotionTimeStamp for non-existent Branch ID: {branchId}");
                }
                 // Optional: Log success
                 // await _databaseLogger.LogServiceActionAsync($"Updated LastPromotionTimeStamp for Branch ID: {branchId} to {timestamp:O}");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("BRANCH_REPOSITORY", $"Error updating LastPromotionTimeStamp for Branch ID: {branchId}", ex);
                throw;
            }
        }

        public async Task UpdateLastPriceTimestampAsync(long branchId, DateTime timestamp)
        {
             try
            {
                int affectedRows = await _context.Branches
                    .Where(b => b.Id == branchId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(b => b.LastPriceTimeStamp, timestamp));

                 if (affectedRows == 0)
                {
                     // Log as a service action instead of a warning
                     await _databaseLogger.LogServiceActionAsync($"Warning: Attempted to update LastPriceTimeStamp for non-existent Branch ID: {branchId}");
                }
                // Optional: Log success
                // await _databaseLogger.LogServiceActionAsync($"Updated LastPriceTimeStamp for Branch ID: {branchId} to {timestamp:O}");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("BRANCH_REPOSITORY", $"Error updating LastPriceTimeStamp for Branch ID: {branchId}", ex);
                throw;
            }
        }
    }
}
