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
        /// <summary>
        /// Gets all catalog items from Comax API
        /// </summary>
        Task<IList<CatalogItemDto>> GetCatalogItemsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets catalog items for specific barcodes from Comax API
        /// </summary>
        Task<IList<CatalogItemDto>> GetCatalogItemsByBarcodesAsync(Branch branch, IEnumerable<string> barcodes, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets catalog XML for specific barcodes from Comax API, handling barcode input in different ways
        /// </summary>
        Task<string> GetCatalogXmlForBarcodesAsync(Branch branch, IEnumerable<string> barcodes, bool useItemId = true, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets promotions from Comax API
        /// </summary>
        Task<PromotionDto> GetPromotionsAsync(Branch branch, DateTime lastUpdateDate, bool justActive = false, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Builds the URL for the Comax catalog API
        /// </summary>
        string BuildComaxCatalogUrl(Configuration config, Branch branch, DateTime LastUpdateDate);

        /// <summary>
        /// Gets the catalog XML from Comax API
        /// </summary>
        Task<string> GetCatalogXmlAsync(Branch branch, DateTime lastUpdateDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the catalog XML from Comax API
        /// </summary>
        Task<string> GetCatalogNewXmlAsync(Branch branch, DateTime lastUpdateDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets new prices from Comax API
        /// </summary>
        Task<List<ItemSalePriceDto>> GetNewPricesAsync(Branch branch, DateTime fromDate, CancellationToken cancellationToken = default);
    }
}
