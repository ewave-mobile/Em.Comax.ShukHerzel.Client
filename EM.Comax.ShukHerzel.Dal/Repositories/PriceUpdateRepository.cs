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
        public PriceUpdateRepository(ShukHerzelEntities context) : base(context)
        {
        }

        public async Task DeleteTransferredItemsOlderThanAsync(int days)
        {
            //delete all older that days
            var items = await _context.PriceUpdates.Where(x => x.CreatedDateTime < DateTime.Now.AddDays(-days)).ToListAsync();
            await _context.BulkDeleteAsync(items);
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
