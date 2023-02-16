using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityReport
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityReportsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreFinalControlUnsuitabilityReportsRepository : EfCoreRepository<FinalControlUnsuitabilityReports>, IFinalControlUnsuitabilityReportsRepository
    {
        public EFCoreFinalControlUnsuitabilityReportsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
