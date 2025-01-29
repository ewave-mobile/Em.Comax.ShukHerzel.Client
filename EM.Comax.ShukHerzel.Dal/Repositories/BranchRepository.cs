using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ShukHerzelEntities context) : base(context)
        {
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesByCompanyIdAsync(long companyId)
        {
            return await Task.Run(() => _context.Branches.Where(b => b.CompanyId == companyId).ToList());
        }
    }
}
