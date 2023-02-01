using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.MaintenancePeriod;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenancePeriod
{
    [ServiceRegistration(typeof(IMaintenancePeriodsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreMaintenancePeriodsRepository : EfCoreRepository<MaintenancePeriods>, IMaintenancePeriodsRepository
    {
        public EFCoreMaintenancePeriodsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
