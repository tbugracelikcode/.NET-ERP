using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.UnplannedMaintenance;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnplannedMaintenance
{
    public interface IUnplannedMaintenancesRepository : IEfCoreRepository<UnplannedMaintenances>
    {
    }
}
