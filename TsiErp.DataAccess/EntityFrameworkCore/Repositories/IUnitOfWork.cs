using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChanges();
    }
}
