using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrderLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrderLine
{
    [ServiceRegistration(typeof(ISalesOrderLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreSalesOrderLinesRepository : EfCoreRepository<SalesOrderLines>, ISalesOrderLinesRepository
    {
        public EFCoreSalesOrderLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
