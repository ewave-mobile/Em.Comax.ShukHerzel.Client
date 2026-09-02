using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IPriceUpdateRepository : IBaseRepository<PriceUpdate>
    {
        Task<List<PriceUpdate>> GetNonTransferredItemsAsync();

        /// <summary>
        /// Marks these items as transferred in the temp table.
        /// </summary>
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
        Task RemoveDuplicateItemsAsync();
        Task DeleteTransferredItemsOlderThanAsync(int days);

        /// <summary>
        /// Latest non-transferred, non-approved PriceUpdate per barcode for the branch.
        /// Optional <paramref name="createdDate"/> filters by CreatedDateTime calendar day (local).
        /// </summary>
        Task<List<PriceUpdate>> GetPendingApprovalByBranchAsync(long branchId, DateTime? createdDate = null);

        /// <summary>
        /// Sets IsOperTransferApproved, OperTransferApprovedDate and OperTransferApprovedByUserId for the given IDs.
        /// </summary>
        Task ApproveOperTransferAsync(IEnumerable<long> ids, int? approvedByUserId);
    }
}
