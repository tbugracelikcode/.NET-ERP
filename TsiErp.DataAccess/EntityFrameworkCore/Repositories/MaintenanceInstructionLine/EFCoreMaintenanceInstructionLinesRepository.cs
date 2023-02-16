using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstructionLine
{
    [ServiceRegistration(typeof(IMaintenanceInstructionLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreMaintenanceInstructionLinesRepository : EfCoreRepository<MaintenanceInstructionLines>, IMaintenanceInstructionLinesRepository
    {
        public EFCoreMaintenanceInstructionLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
