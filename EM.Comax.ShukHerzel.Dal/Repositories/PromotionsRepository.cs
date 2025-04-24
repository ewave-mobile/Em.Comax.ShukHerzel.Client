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
    public class PromotionsRepository: BaseRepository<Promotion>, IPromotionsRepository
    {
        public PromotionsRepository(ShukHerzelEntities context) : base(context)
        {
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
            //delete transferred promotions older than retention days
            var olderThan = DateTime.Now.AddDays(-retentionDays);
            await _context.BulkDeleteAsync( _context.Promotions.Where(x => (x.IsTransferredToOper == true && x.TransferredDateTime < olderThan) || x.CreatedDateTime < olderThan));
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
    }
}
