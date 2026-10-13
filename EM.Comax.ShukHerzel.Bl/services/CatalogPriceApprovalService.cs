using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class CatalogPriceApprovalService : ICatalogPriceApprovalService
    {
        private readonly IAllItemsNewRepository _allItemsNewRepository;

        public CatalogPriceApprovalService(IAllItemsNewRepository allItemsNewRepository)
        {
            _allItemsNewRepository = allItemsNewRepository;
        }

        public Task<List<PendingCatalogPriceRow>> GetPendingByBranchAsync(long branchId, DateTime? createdDate = null)
        {
            return _allItemsNewRepository.GetCatalogPricePendingByBranchAsync(branchId, createdDate);
        }

        public Task ApproveAsync(IEnumerable<long> ids, int? approvedByUserId)
        {
            return _allItemsNewRepository.ApproveCatalogPriceAsync(ids, approvedByUserId);
        }

        public async Task ThrowIfBlockedAsync(IEnumerable<(string Code, long BranchId)> codes)
        {
            var codeList = codes?
                .Where(code => !string.IsNullOrEmpty(code.Code))
                .ToList()
                ?? new List<(string Code, long BranchId)>();
            if (codeList.Count == 0)
            {
                return;
            }

            var pending = await _allItemsNewRepository.GetPendingCatalogKeysAsync();
            var lookup = new HashSet<(string Code, long BranchId)>();
            foreach (var key in pending)
            {
                if (!string.IsNullOrEmpty(key.Barcode))
                {
                    lookup.Add((key.Barcode, key.BranchId));
                }

                if (!string.IsNullOrEmpty(key.XmlId))
                {
                    lookup.Add((key.XmlId, key.BranchId));
                }
            }

            var blocked = codeList
                .Where(code => lookup.Contains((code.Code, code.BranchId)))
                .Select(code => code.Code)
                .Distinct()
                .ToList();
            if (blocked.Count > 0)
            {
                throw new CatalogPriceApprovalBlockedException(blocked);
            }
        }
    }
}
