using EM.Comax.ShukHerzel.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface ITrailingItemRepository : IBaseRepository<TrailingItem>
    {
        /// <summary>
        /// Gets a trailing item by its Kod
        /// </summary>
        /// <param name="kod">The Kod to search for</param>
        /// <returns>The trailing item if found, null otherwise</returns>
        Task<TrailingItem?> GetByKodAsync(string kod);

        /// <summary>
        /// Gets multiple trailing items by their Kods
        /// </summary>
        /// <param name="kods">List of Kods to search for</param>
        /// <returns>Dictionary with Kod as key and TrailingItem as value</returns>
        Task<Dictionary<string, TrailingItem>> GetByKodsAsync(IEnumerable<string> kods);
    }
}