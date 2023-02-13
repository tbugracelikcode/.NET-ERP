using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Respositories.UnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;

namespace TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TsiErpDbContext _dbContext;

        public UnitOfWork()
        {
            _dbContext = new TsiErpDbContext();
        }

        #region Dispose Method
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region SaveChanges Method
        public async Task<int> SaveChanges()
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    int returnValue =await _dbContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return returnValue;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }
        #endregion

        private EFCoreBranchesRepository _branchRepository;

        public EFCoreBranchesRepository BranchRepository => _branchRepository ?? (_branchRepository = new EFCoreBranchesRepository(_dbContext));
    }
}
