using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IOperativeService
    {
        /// <summary>
        /// Synchronizes temp AllItems + Promotions to the final Item table
        /// </summary>
        Task SyncAllItemsAndPromotionsAsync(IProgress<string> progress, CancellationToken cancellationToken = default);
        Task SyncAllItemsNewAndPromotionsAsync(IProgress<string> progress, CancellationToken cancellationToken = default);
  
        /// <summary>
        /// Searches for items in the operative table based on the provided criteria
        /// </summary>
        Task<List<Models.Models.Item>> SearchItemsAsync(string barcode = null, long? branchId = null, string name = null);
        
        /// <summary>
        /// Sets an item's IsSentToEsl flag to false so it will be sent in the next service iteration
        /// </summary>
        Task SetItemNotSentAsync(long itemId);
        
        /// <summary>
        /// Removes promotion details from an item and sets it to be resent
        /// </summary>
        Task RemovePromotionAsync(long itemId);
    }
}
