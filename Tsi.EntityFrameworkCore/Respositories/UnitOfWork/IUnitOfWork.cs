using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.EntityFrameworkCore.Repositories;

namespace Tsi.EntityFrameworkCore.Respositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChanges();
    }
}
