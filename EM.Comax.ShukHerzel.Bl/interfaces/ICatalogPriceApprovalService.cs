using EM.Comax.ShukHerzel.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface ICatalogPriceApprovalService
    {
        Task<List<PendingCatalogPriceRow>> GetPendingByBranchAsync(long branchId, DateTime? createdDate = null);
        Task ApproveAsync(IEnumerable<long> ids, int? approvedByUserId);
        Task ThrowIfBlockedAsync(IEnumerable<(string Code, long BranchId)> codes);
    }
}
