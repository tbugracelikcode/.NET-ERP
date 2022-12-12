using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseUnsuitabilityReport
{
    [ServiceRegistration(typeof(IPurchaseUnsuitabilityReportsRepository), DependencyInjectionType.Scoped)]
    public class EFCorePurchaseUnsuitabilityReportsRepository : EfCoreRepository<PurchaseUnsuitabilityReports>, IPurchaseUnsuitabilityReportsRepository
    {
        public EFCorePurchaseUnsuitabilityReportsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
