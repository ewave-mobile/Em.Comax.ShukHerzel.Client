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
    public class AllItemsRepository : BaseRepository<AllItem>, IAllItemsRepository
    {
        private readonly IDatabaseLogger _databaseLogger;
        public AllItemsRepository(ShukHerzelEntities context, IDatabaseLogger databaseLogger) : base(context)
        {
            _databaseLogger = databaseLogger;
        }

        public async Task<List<AllItem>> GetNonTransferredItemsAsync()
        {
            //get all non transferred items
            return await _context.AllItems.Where(x => x.IsTransferredToOper == false && x.IsBad == false).ToListAsync();
        }
        public async Task RemoveDuplicateItemsAsync()
        {
            string sql = @"
        WITH CTE AS (
            SELECT 
                Id,
                ROW_NUMBER() OVER (PARTITION BY Barcode, BranchId ORDER BY CreatedDateTime DESC) AS rn
            FROM 
                temp.AllItems
            WHERE 
                IsTransferredToOper = 0
        )
        DELETE FROM temp.AllItems
        WHERE Id IN (SELECT Id FROM CTE WHERE rn > 1);
    ";


            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql);
                await _databaseLogger.LogServiceActionAsync("Duplicate operative AllItems removed successfully.");
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

                var itemsToUpdate = batchIds.Select(id => new AllItem
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
            //delete all items that are transferred and older than the given days
            var olderThan = DateTime.Now.AddDays(-days);
            var itemsToDelete = await _context.AllItems.Where(x => (x.IsTransferredToOper && x.TransferredDateTime < olderThan) || x.CreatedDateTime < olderThan ).ToListAsync();

            if (itemsToDelete.Count > 0)
            {
                await _context.BulkDeleteAsync(itemsToDelete);
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

                var itemsToUpdate = batchIds.Select(id => new AllItem
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
