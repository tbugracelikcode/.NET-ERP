using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Operation;


namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Operation
{
    [ServiceRegistration(typeof(IOperationsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreOperationsRepository : EfCoreRepository<Operations>, IOperationsRepository
    {
        public EFCoreOperationsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
