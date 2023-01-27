using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.StationInventory;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationInventory
{
    public interface IStationInventoriesRepository : IEfCoreRepository<StationInventories>
    {
    }
}
