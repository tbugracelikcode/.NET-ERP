using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace TsiErp.DataAccess.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
       
        private readonly TsiErpDbContext _context;

        private IDbContextTransaction _transation;

        private bool _disposed;

       
        public UnitOfWork(TsiErpDbContext context)
        {
            _context = context;
        }

        public IEfCoreRepository<T> GetRepository<T>() where T : class,IEntity
        {
            return new EfCoreRepository<T>(_context);
        }

        public int SaveChanges()
        {
            var transaction = _transation != null ? _transation : _context.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    if (_context == null)
                    {
                        throw new ArgumentException("Context is null");
                    }

                    int result = _context.SaveChanges();


                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
