using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseRequest;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequest
{
    [ServiceRegistration(typeof(IPurchaseRequestsRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchaseRequestsRepository : EfCoreRepository<PurchaseRequests>, IPurchaseRequestsRepository
    {
        public EFCorePurchaseRequestsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
