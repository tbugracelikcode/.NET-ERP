using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tsi.EntityFrameworkCore.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        
        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(Guid id);
    }
}
