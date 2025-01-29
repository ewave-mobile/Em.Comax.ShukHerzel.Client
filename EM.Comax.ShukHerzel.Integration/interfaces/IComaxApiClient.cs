using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Integration.interfaces
{
    public interface IComaxApiClient
    {
        Task<IList<CatalogItemDto>> GetCatalogItemsAsync(CancellationToken cancellationToken = default);
        Task<PromotionDto> GetPromotionsAsync(Branch branch, DateTime lastUpdateDate, bool justActive = false, CancellationToken cancellationToken = default);
        string BuildComaxCatalogUrl(Configuration config, Branch branch, DateTime LastUpdateDate);

        Task<string> GetCatalogXmlAsync(Branch branch, DateTime lastUpdateDate, CancellationToken cancellationToken = default);
    }
}
