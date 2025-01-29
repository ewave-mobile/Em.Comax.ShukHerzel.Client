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
        //get all branches by company id
        Task<IEnumerable<Branch>> GetAllBranchesByCompanyIdAsync(long companyId);
    }
}
