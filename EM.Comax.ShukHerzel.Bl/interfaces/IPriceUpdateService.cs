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
    }
}
