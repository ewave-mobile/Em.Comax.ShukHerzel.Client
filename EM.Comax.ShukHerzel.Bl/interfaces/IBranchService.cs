using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IBranchService
    {
        Task<List<Branch>> GetAllBranchesByCompany(long companyId);
        Task<List<Branch>> GetAllBranches();
    }
}
