// BadItemLogRepository.cs
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class BadItemLogRepository :BaseRepository<BadItemLog>, IBadItemLogRepository
    {
        public BadItemLogRepository(ShukHerzelEntities context) : base(context)
        {
        }

        public async Task BulkInsertBadItemsAsync(IEnumerable<BadItemLog> badItems, int batchSize = 1000)
        {
            await _context.BulkInsertAsync(badItems, new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = true,
                BatchSize = batchSize
            });
        }

        public Task DeleteLogsOlderThanAsync(int days)
        {
            //delete older than days
            return _context.Database.ExecuteSqlRawAsync("DELETE FROM Log.BadItemLog WHERE DATEDIFF(DAY, CreatedAt, GETDATE()) > {0}", days);
        }
    }
}
