using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IAllItemsNewRepository : IBaseRepository<AllItemComax>
    {
        /// <summary>
        /// Gets all non-transferred items from the temp table.
        /// </summary>
        Task<List<AllItemComax>> GetNonTransferredItemsAsync();

        /// <summary>
        /// Marks these items as transferred in the temp table.
        /// </summary>
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
        Task RemoveDuplicateItemsAsync();
        Task DeleteTransferredItemsOlderThanAsync(int days);
        Task MarkAsBad(IEnumerable<long> ids);

        Task MarkCatalogPricePendingAsync(IEnumerable<long> ids);
        Task<List<PendingCatalogPriceRow>> GetCatalogPricePendingByBranchAsync(long branchId, DateTime? createdDate = null);
        Task ApproveCatalogPriceAsync(IEnumerable<long> ids, int? approvedByUserId);
        Task<List<(string Barcode, string XmlId, long BranchId)>> GetPendingCatalogKeysAsync();
    }
}
