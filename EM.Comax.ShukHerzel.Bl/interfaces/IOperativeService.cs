using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Bl.interfaces
{
    public interface IOperativeService
    {
        /// <summary>
        /// Synchronizes temp AllItems + Promotions to the final Item table
        /// </summary>
        Task SyncAllItemsAndPromotionsAsync(IProgress<string> progress, CancellationToken cancellationToken = default);
    }
}
