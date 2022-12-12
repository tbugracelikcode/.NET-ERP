using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityReport
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityReportsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreOperationUnsuitabilityReportsRepository : EfCoreRepository<OperationUnsuitabilityReports>, IOperationUnsuitabilityReportsRepository
    {
        public EFCoreOperationUnsuitabilityReportsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
