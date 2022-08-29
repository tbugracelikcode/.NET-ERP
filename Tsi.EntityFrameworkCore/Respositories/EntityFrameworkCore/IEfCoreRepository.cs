using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;

namespace Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore
{
    public interface IEfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors);
    }
}
