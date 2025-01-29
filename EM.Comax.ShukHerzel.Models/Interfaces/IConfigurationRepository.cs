using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IConfigurationRepository : IBaseRepository<Configuration>
    {
         Task<Configuration> getCompanyConfig(long companyId);
    }
}
