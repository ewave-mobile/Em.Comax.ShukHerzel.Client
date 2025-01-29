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

        public async Task DeleteOldOperativeRecordsAsync(int days)
        {
            // delete all sent to esl older than days while keeping one unique barcode per branchid 
            string sql = $@"
                DELETE FROM oper.items
                WHERE IsSentToEsl = 1
                AND CreateDateTime < DATEADD(day, -{days}, GETDATE())
                AND Id NOT IN (
                    SELECT Id
                    FROM (
                        SELECT Id, ROW_NUMBER() OVER (PARTITION BY Barcode, BranchId ORDER BY CreateDateTime DESC) AS rn
                        FROM oper.items
                    ) AS t
                    WHERE rn = 1
                );
            ";

            try
            {
                await _context.Database.ExecuteSqlRawAsync(sql);
                await _databaseLogger.LogServiceActionAsync("Old operative Items removed successfully.");
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("OPERATIVE_SERVICE", "DeleteOldOperativeRecordsAsync", ex);
                throw; // Re-throw to handle upstream if necessary
            }
        }

        public async Task CleanExpiredPromotions()
        {
            //get all items in the items table that have expired todate field and delete all promotion related data and set is sent to esl false
            var now = DateTime.Now;
            var expiredPromotions = await _context.Items
                .Where(x => x.IsPromotion && x.PromotionToDate < now)
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
    }
}
     
    
