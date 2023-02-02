using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenanceLine
{
    public interface IUnplannedMaintenanceLinesRepository : IEfCoreRepository<UnplannedMaintenanceLines>
    {
    }
}
