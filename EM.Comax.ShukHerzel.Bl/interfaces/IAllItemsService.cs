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
        Task InsertCatalogAsync( Branch branch,DateTime? lastUpdateDate ,IProgress<string>? progress = null,CancellationToken cancellationToken = default );
        List<ArrayOfClsItemsClsItems> DeserializeCatalogItems(string xml);
        Task<List<AllItem>> MapToAllItems(List<ArrayOfClsItemsClsItems> clsItems, DateTime createDate , Branch branch, Guid operationGuid);
        Task InsertAllItemsAsync(List<AllItem> allItems);
        Task<List<AllItem>> GetNonTransferredItemsAsync();
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
    }
}
