using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem
{
    [ServiceRegistration(typeof(IProductionOrderChangeItemsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductionOrderChangeItemsRepository : EfCoreRepository<ProductionOrderChangeItems>, IProductionOrderChangeItemsRepository
    {
        public EFCoreProductionOrderChangeItemsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
