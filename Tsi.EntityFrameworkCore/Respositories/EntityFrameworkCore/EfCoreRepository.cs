using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.EntityFrameworkCore.Repositories;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.EntityFrameworkCore.Respositories.Extensions;

namespace Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore
{
    public class EfCoreRepository<TDbContext, TEntity> : IEfCoreRepository<TEntity>
        where TDbContext : DbContext, new()
        where TEntity : class, IEntity, new()
    {

        private DbContext _dbContext;

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            var entity = await _dbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);

            return entity;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            var queryable = await WithDetailsAsync(includeProperties);

            TEntity entity;

            if (predicate != null)
            {
                return await queryable.FirstOrDefaultAsync(predicate);
            }

            return await queryable.FirstOrDefaultAsync();

        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            return predicate == null ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            var queryable = await WithDetailsAsync(includeProperties);

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return await queryable.ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            var addedEntity = _dbContext.Entry(entity);
            addedEntity.State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
            return addedEntity.Entity;

        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            var updatedEntity = _dbContext.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return updatedEntity.Entity;

        }

        public async Task DeleteAsync(Guid id)
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            var entity = _dbContext.Set<TEntity>().Single(t => t.Id == id);

            if (entity != null)
            {
                var deletedEntity = _dbContext.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return IncludeDetails(await GetQueryableAsync(), propertySelectors);
        }

        public async Task<DbSet<TEntity>> GetDbSetAsync()
        {
            if (_dbContext == null)
                _dbContext = GetDbContext();

            return _dbContext.Set<TEntity>();

        }


        private DbContext GetDbContext()
        {
            return new TDbContext();
        }


        private async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            return (await GetDbSetAsync()).AsQueryable();
        }

        private static IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors != null)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }
    }
}
