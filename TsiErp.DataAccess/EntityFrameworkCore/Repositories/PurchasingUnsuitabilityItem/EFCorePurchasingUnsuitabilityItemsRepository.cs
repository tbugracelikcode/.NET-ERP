using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasingUnsuitabilityItem
{
    [ServiceRegistration(typeof(IPurchasingUnsuitabilityItemsRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchasingUnsuitabilityItemsRepository : EfCoreRepository<PurchasingUnsuitabilityItems>, IPurchasingUnsuitabilityItemsRepository
    {
        public EFCorePurchasingUnsuitabilityItemsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
