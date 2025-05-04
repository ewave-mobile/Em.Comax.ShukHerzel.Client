using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {
        Task<IEnumerable<Branch>> GetAllBranchesByCompanyIdAsync(long companyId);

        // Methods for specific timestamp updates
        Task UpdateLastCatalogTimestampAsync(long branchId, DateTime timestamp);
        Task UpdateLastPromotionTimestampAsync(long branchId, DateTime timestamp);
        Task UpdateLastPriceTimestampAsync(long branchId, DateTime timestamp);
    }
}
