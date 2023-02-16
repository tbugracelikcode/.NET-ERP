using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.PurchasePriceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePriceLine
{
    [ServiceRegistration(typeof(IPurchasePriceLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchasePriceLinesRepository : EfCoreRepository<PurchasePriceLines>, IPurchasePriceLinesRepository
    {
        public EFCorePurchasePriceLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
