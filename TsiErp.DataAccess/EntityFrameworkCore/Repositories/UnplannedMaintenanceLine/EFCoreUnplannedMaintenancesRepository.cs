using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenanceLine
{
    [ServiceRegistration(typeof(IUnplannedMaintenanceLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreUnplannedMaintenanceLinesRepository : EfCoreRepository<UnplannedMaintenanceLines>, IUnplannedMaintenanceLinesRepository
    {
        public EFCoreUnplannedMaintenanceLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
