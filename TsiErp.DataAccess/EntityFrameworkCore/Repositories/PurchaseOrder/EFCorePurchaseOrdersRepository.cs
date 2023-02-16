using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.PurchaseOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrder
{
    [ServiceRegistration(typeof(IPurchaseOrdersRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchaseOrdersRepository : EfCoreRepository<PurchaseOrders>, IPurchaseOrdersRepository
    {
        public EFCorePurchaseOrdersRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
