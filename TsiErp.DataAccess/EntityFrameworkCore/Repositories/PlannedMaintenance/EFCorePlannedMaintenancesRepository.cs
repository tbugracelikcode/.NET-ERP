using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenance;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PlannedMaintenance
{
    [ServiceRegistration(typeof(IPlannedMaintenancesRepository), DependencyInjectionType.Scoped)]
    public class EFCorePlannedMaintenancesRepository : EfCoreRepository<PlannedMaintenances>, IPlannedMaintenancesRepository
    {
        public EFCorePlannedMaintenancesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
