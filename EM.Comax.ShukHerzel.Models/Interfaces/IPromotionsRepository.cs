using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IPromotionsRepository : IBaseRepository<Promotion>
    {
        Task DeleteTransferredItemsOlderThanAsync(int retentionDays);
        Task<List<Promotion>> GetNonTransferredPromotionsAsync();
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
        Task DeleteExpiredPromotionsAsync();
        
        // New methods for data management
        Task<List<Promotion>> SearchPromotionsAsync(string kod = null, string itemKod = null, long? branchId = null);
        Task SetPromotionNotTransferredAsync(long promotionId);
    }
}
