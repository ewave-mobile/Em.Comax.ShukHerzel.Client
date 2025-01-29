using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IPromotionsService
    {
        Task InsertPromotionsAsync(
         Branch branch,
         DateTime? lastUpdateDate,
         IProgress<string>? progress = null,
         CancellationToken cancellationToken = default
     );
        Task<List<Promotion>> GetNonTransferredPromotionsAsync();
        Task MarkAsTransferredAsync(IEnumerable<long> ids, DateTime transferredTime);
    }
}
