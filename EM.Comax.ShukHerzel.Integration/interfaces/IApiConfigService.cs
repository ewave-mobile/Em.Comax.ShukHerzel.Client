using EM.Comax.ShukHerzel.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Integration.interfaces
{
    public interface IApiConfigService
    {
        ApiConfig GetApiConfig(string apiName);
    }
}
