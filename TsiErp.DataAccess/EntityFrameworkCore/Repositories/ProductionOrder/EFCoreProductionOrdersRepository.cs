using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductionOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder
{
    [ServiceRegistration(typeof(IProductionOrdersRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductionOrdersRepository : EfCoreRepository<ProductionOrders>, IProductionOrdersRepository
    {
        public EFCoreProductionOrdersRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
