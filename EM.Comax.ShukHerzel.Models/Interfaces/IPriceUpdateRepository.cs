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
    }
}
