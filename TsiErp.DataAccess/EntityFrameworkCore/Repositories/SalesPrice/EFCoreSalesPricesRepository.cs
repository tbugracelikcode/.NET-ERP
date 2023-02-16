using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesOrderLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPrice
{
    [ServiceRegistration(typeof(ISalesPricesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreSalesPricesRepository : EfCoreRepository<SalesPrices>, ISalesPricesRepository
    {
        public EFCoreSalesPricesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
