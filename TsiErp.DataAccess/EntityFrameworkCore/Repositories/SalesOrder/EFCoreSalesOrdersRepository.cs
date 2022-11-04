using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesProposition;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder
{
    [ServiceRegistration(typeof(ISalesOrdersRepository), DependencyInjectionType.Scoped)]
    public class EFCoreSalesOrdersRepository : EfCoreRepository<SalesOrders>, ISalesOrdersRepository
    {
        public EFCoreSalesOrdersRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
