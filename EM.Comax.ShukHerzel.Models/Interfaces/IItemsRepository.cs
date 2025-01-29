using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IItemsRepository : IBaseRepository<Item>
    {
        Task<Item> FindByKodAndBranchAsync(string kod, long branchId);
        Task<List<Item>> GetItemsByBarcodesAndBranchIdsAsync(IEnumerable<(string Barcode, long BranchId)> keys);
        Task RemoveDuplicateItemsAsync();
        Task<IEnumerable<ItemWithBranch>> GetItemWithBranches();
        Task<List<Item>> GetItemsToSendAsync();
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredDateTime);
        Task DeleteOldOperativeRecordsAsync(int days);

        Task CleanExpiredPromotions();
    }
}
