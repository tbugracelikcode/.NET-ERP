using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.SalesProposition;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction
{
    [ServiceRegistration(typeof(IMaintenanceInstructionsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreMaintenanceInstructionsRepository : EfCoreRepository<MaintenanceInstructions>, IMaintenanceInstructionsRepository
    {
        public EFCoreMaintenanceInstructionsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
