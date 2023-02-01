using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstructionLine
{
    public interface IMaintenanceInstructionLinesRepository : IEfCoreRepository<MaintenanceInstructionLines>
    {
    }
}
