using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class ConfigurationRepository :BaseRepository<Configuration>,  IConfigurationRepository
    {
        public ConfigurationRepository(ShukHerzelEntities context) : base(context)
        {
        }

        public async Task<Configuration> getCompanyConfig(long companyId)
        {
           return await _context.Configurations.Where(x => x.CompanyId == companyId).FirstOrDefaultAsync();
        }
    }
}
