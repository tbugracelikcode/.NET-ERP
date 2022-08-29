using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;

namespace TsiErp.DataAccess.EntityFrameworkCore.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEfCoreRepository<T> GetRepository<T>() where T : class, IEntity;

        int SaveChanges();
    }
}
