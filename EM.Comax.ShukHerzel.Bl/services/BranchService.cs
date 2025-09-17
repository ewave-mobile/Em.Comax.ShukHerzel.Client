using EM.Comax.ShukHerzel.Bl.interfaces;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }
        public async Task<List<Branch>> GetAllBranchesByCompany(long companyId)
        {
            var branches = await _branchRepository.GetAllBranchesByCompanyIdAsync(companyId);
            return branches.ToList();
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            var branches = await _branchRepository.GetAllAsync();
            return branches.ToList();
        }
    }
}
