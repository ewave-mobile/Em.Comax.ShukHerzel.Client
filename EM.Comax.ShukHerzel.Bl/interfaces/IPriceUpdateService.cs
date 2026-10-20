using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IPriceUpdateService
    {
        /// <summary>
        /// Retrieves new price updates from Comax and inserts them into the DB.
        /// </summary>
        Task InsertPriceUpdatesAsync(
            Branch branch,
            DateTime? lastUpdateDate,
            IProgress<string>? progress = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all PriceUpdate records that have not yet been transferred.
        /// </summary>
        Task<List<PriceUpdate>> GetNonTransferredPriceUpdatesAsync();

        /// <summary>
        /// Marks the provided price update IDs as transferred.
        /// </summary>
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);

        /// <summary>
        /// Latest non-transferred, non-approved PriceUpdate per barcode for the branch.
        /// Optional <paramref name="createdDate"/> filters by CreatedDateTime calendar day (local).
        /// </summary>
        Task<List<PriceUpdate>> GetPendingApprovalByBranchAsync(long branchId, DateTime? createdDate = null);

        /// <summary>
        /// Approves Oper transfer for the given price update IDs.
        /// </summary>
        Task ApproveOperTransferAsync(IEnumerable<long> ids, int? approvedByUserId);
        Task RejectOperTransferAsync(IEnumerable<long> ids, int? rejectedByUserId);
    }
}
