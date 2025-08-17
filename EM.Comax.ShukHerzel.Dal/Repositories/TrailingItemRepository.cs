using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class TrailingItemRepository : BaseRepository<TrailingItem>, ITrailingItemRepository
    {
        public TrailingItemRepository(ShukHerzelEntities context) : base(context)
        {
        }

        /// <summary>
        /// Gets a trailing item by its Kod
        /// </summary>
        /// <param name="kod">The Kod to search for</param>
        /// <returns>The trailing item if found, null otherwise</returns>
        public async Task<TrailingItem?> GetByKodAsync(string kod)
        {
            if (string.IsNullOrWhiteSpace(kod))
                return null;

            return await _context.TrailingItems
                .Where(t => t.Kod == kod)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets multiple trailing items by their Kods
        /// </summary>
        /// <param name="kods">List of Kods to search for</param>
        /// <returns>Dictionary with Kod as key and TrailingItem as value</returns>
        public async Task<Dictionary<string, TrailingItem>> GetByKodsAsync(IEnumerable<string> kods)
        {
            if (kods == null || !kods.Any())
                return new Dictionary<string, TrailingItem>();

            var validKods = kods.Where(k => !string.IsNullOrWhiteSpace(k)).ToList();
            if (!validKods.Any())
                return new Dictionary<string, TrailingItem>();

            var trailingItems = await _context.TrailingItems
                .Where(t => validKods.Contains(t.Kod))
                .ToListAsync();

            return trailingItems.ToDictionary(t => t.Kod, t => t);
        }
    }
}