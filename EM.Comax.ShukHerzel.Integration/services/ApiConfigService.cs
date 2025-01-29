using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.CustomModels;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Integration.services
{
    public class ApiConfigService : IApiConfigService
    {
        private readonly ShukHerzelEntities _dbContext;

        public ApiConfigService(ShukHerzelEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public ApiConfig GetApiConfig(string apiName)
        {
            // Query [management].[ApiConfiguration] table by apiName
            var row = _dbContext.Configurations
                .Where(x => x.CompanyId == ShukHerzl.Common.Constants.SHUK_HERZEL_COMPANY_ID )
                .FirstOrDefault();

            if (row == null)
                throw new Exception($"API config not found for '{apiName}'.");

            return new ApiConfig
            {
                EslApiKey = row.EslApiKey,
                EslBaseUrl = row.EslUrl,
                ComaxBaseUrl = row.ComaxApiUrl
            };
        }
    }

}
