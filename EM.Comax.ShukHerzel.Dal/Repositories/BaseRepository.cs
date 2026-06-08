using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var entityList = entities as IList<T> ?? entities.ToList();
            TruncateStringProperties(entityList);
            await _context.BulkInsertAsync(entityList);
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
            var entityList = entities as IList<T> ?? entities.ToList();
            TruncateStringProperties(entityList);
            await _context.BulkInsertOrUpdateAsync(entityList, config);
        }

        private void TruncateStringProperties(IEnumerable<T> entities)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            if (entityType == null)
            {
                return;
            }

            var stringProperties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string) && p.GetMaxLength() is int maxLength && maxLength > 0)
                .Select(p => (Property: p, MaxLength: p.GetMaxLength()!.Value, PropertyInfo: p.PropertyInfo))
                .Where(x => x.PropertyInfo != null)
                .ToList();

            if (stringProperties.Count == 0)
            {
                return;
            }

            foreach (var entity in entities)
            {
                foreach (var (_, maxLength, propertyInfo) in stringProperties)
                {
                    var value = (string?)propertyInfo!.GetValue(entity);
                    if (value != null && value.Length > maxLength)
                    {
                        propertyInfo.SetValue(entity, value[..maxLength]);
                    }
                }
            }
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
