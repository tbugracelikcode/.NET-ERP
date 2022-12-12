using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseRequestLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequestLine
{
    [ServiceRegistration(typeof(IPurchaseRequestLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchaseRequestLinesRepository : EfCoreRepository<PurchaseRequestLines>, IPurchaseRequestLinesRepository
    {
        public EFCorePurchaseRequestLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
