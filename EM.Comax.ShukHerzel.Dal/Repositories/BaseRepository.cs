using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace EM.Comax.ShukHerzel.Dal.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ShukHerzelEntities _context;

        public BaseRepository(ShukHerzelEntities context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            try
            {

                await _context.BulkInsertAsync(entities);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public async Task BulkUpdateAsync(IEnumerable<T> entities)
        {
            await _context.BulkUpdateAsync(entities);

        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task BulkInsertOrUpdateAsync(IEnumerable<T> entities, BulkConfig config)
        {
            await _context.BulkInsertOrUpdateAsync(entities, config);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task BulkDeleteAsync(IEnumerable<T> entities)
        {
            await _context.BulkDeleteAsync(entities);
        }
    }
}
