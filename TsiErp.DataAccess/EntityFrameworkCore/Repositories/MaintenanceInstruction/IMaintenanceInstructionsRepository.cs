using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.SalesProposition;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction
{
    public interface IMaintenanceInstructionsRepository : IEfCoreRepository<MaintenanceInstructions>
    {
    }
}
