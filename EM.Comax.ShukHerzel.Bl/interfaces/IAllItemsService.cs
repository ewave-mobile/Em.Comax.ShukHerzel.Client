using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IAllItemsService
    {
        /// <summary>
        /// Inserts catalog items from Comax API into the temporary AllItems table
        /// </summary>
        Task InsertCatalogAsync(Branch branch, DateTime? lastUpdateDate, IProgress<string>? progress = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets catalog items from Comax API for specific barcodes and adds them to the temp AllItems table
        /// </summary>
        Task<List<AllItem>> GetItemsForBarcodesAsync(Branch branch, IEnumerable<string> barcodes, IProgress<string>? progress = null);
        
        /// <summary>
        /// Deserializes catalog items from XML
        /// </summary>
        List<ArrayOfClsItemsClsItems> DeserializeCatalogItems(string xml);
        
        /// <summary>
        /// Maps catalog items to AllItems
        /// </summary>
        Task<List<AllItem>> MapToAllItems(List<ArrayOfClsItemsClsItems> clsItems, DateTime createDate, Branch branch, Guid operationGuid);
        
        /// <summary>
        /// Inserts AllItems into the database
        /// </summary>
        Task InsertAllItemsAsync(List<AllItem> allItems);
        
        /// <summary>
        /// Gets AllItems that have not been transferred to the operative table
        /// </summary>
        Task<List<AllItem>> GetNonTransferredItemsAsync();
        
        /// <summary>
        /// Marks AllItems as transferred to the operative table
        /// </summary>
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
    }
}
