using EM.Comax.ShukHerzel.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IBadItemLogRepository : IBaseRepository<BadItemLog>
    {
        /// <summary>
        /// Inserts a collection of bad item logs into the BadItemLog table using bulk operations.
        /// </summary>
        /// <param name="badItems">The collection of bad items to log.</param>
        /// <param name="batchSize">The size of each bulk insert batch.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BulkInsertBadItemsAsync(IEnumerable<BadItemLog> badItems, int batchSize = 1000);

        Task DeleteLogsOlderThanAsync(int days);
    }
}
