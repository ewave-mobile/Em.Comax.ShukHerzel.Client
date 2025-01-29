using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Models.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task BulkInsertAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task BulkUpdateAsync(IEnumerable<T> entities);
        Task BulkInsertOrUpdateAsync(IEnumerable<T> entities, BulkConfig config);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
