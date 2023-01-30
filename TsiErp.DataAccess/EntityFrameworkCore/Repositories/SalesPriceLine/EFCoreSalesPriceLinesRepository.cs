using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesPriceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPriceLine
{
    [ServiceRegistration(typeof(ISalesPriceLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreSalesPriceLinesRepository : EfCoreRepository<SalesPriceLines>, ISalesPriceLinesRepository
    {
        public EFCoreSalesPriceLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
