using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseOrderLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrderLine
{
    [ServiceRegistration(typeof(IPurchaseOrderLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchaseOrderLinesRepository : EfCoreRepository<PurchaseOrderLines>, IPurchaseOrderLinesRepository
    {
        public EFCorePurchaseOrderLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
