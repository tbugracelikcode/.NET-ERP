using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchasePrice;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePrice
{
    [ServiceRegistration(typeof(IPurchasePricesRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchasePricesRepository : EfCoreRepository<PurchasePrices>, IPurchasePricesRepository
    {
        public EFCorePurchasePricesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
