using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories
{
    public interface IEfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        Task<TEntity> LockRow(Guid id, bool lockRow, Guid userId);

        Task<IList<TEntity>> FromSqlRawAsync(string sql, params object[] parameters);
    }
}
