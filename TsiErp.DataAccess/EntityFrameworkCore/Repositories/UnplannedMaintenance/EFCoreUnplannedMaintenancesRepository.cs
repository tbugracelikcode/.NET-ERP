using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenance;
using TsiErp.Entities.Entities.UnplannedMaintenance;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenance
{
    [ServiceRegistration(typeof(IUnplannedMaintenancesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreUnplannedMaintenancesRepository : EfCoreRepository<UnplannedMaintenances>, IUnplannedMaintenancesRepository
    {
        public EFCoreUnplannedMaintenancesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
