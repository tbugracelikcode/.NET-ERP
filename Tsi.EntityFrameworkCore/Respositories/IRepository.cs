using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Tsi.Core.Entities.Auditing;

namespace Tsi.EntityFrameworkCore.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true);

        Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = true);

        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true);

        Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = true);

        Task DeleteAsync(Guid id, bool autoSave = true);

        Task<IQueryable<TEntity>> GetQueryableAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
