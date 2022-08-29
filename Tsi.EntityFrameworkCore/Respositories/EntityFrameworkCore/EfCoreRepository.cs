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
    public class EfCoreRepository<TEntity> : IEfCoreRepository<TEntity> where TEntity : class, IEntity
    {

        private DbContext _context;

        private DbSet<TEntity> _dbset;

        public EfCoreRepository(DbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public async Task DeleteAsync(Guid id)
        {
            _dbset.Remove(await GetAsync(t => t.Id == id));
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.FindAsync(predicate);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _dbset.FindAsync(predicate);
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? await _dbset.ToListAsync() : await _dbset.Where(predicate).ToListAsync();
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return predicate == null ? await _dbset.ToListAsync() : await _dbset.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbset.Update(entity);
            return entity;
        }

        public Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            throw new NotImplementedException();
        }
    }
}
