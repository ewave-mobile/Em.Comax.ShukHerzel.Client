using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IPromotionsService
    {
        /// <summary>
        /// Inserts promotions from Comax API into the temporary promotions table
        /// </summary>
        Task InsertPromotionsAsync(
         Branch branch,
         DateTime? lastUpdateDate,
         IProgress<string>? progress = null,
         CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Gets promotions that have not been transferred to the operative table
        /// </summary>
        Task<List<Promotion>> GetNonTransferredPromotionsAsync();
        
        /// <summary>
        /// Marks promotions as transferred to the operative table
        /// </summary>
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
        
        /// <summary>
        /// Searches for promotions based on the provided criteria
        /// </summary>
        Task<List<Promotion>> SearchPromotionsAsync(string kod = null, string itemKod = null, long? branchId = null);
        
        /// <summary>
        /// Sets a promotion's IsTransferredToOper flag to false so it would replace the current promotion
        /// </summary>
        Task SetPromotionNotTransferredAsync(long promotionId);
        
        /// <summary>
        /// Gets promotions from Comax API for specific barcodes and adds them to the temp promotion table
        /// </summary>
        Task<List<Promotion>> GetPromotionsForBarcodesAsync(Branch branch, IEnumerable<string> barcodes, IProgress<string>? progress = null);
    }
}
