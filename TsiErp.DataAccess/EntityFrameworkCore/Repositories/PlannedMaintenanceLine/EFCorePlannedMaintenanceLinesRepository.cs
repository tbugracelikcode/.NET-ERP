using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenanceLine
{
    [ServiceRegistration(typeof(IPlannedMaintenanceLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCorePlannedMaintenanceLinesRepository : EfCoreRepository<PlannedMaintenanceLines>, IPlannedMaintenanceLinesRepository
    {
        public EFCorePlannedMaintenanceLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
