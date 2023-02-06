using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.GrandTotalStockMovement;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.GrandTotalStockMovement
{
    public interface IGrandTotalStockMovementsRepository : IEfCoreRepository<GrandTotalStockMovements>
    {
    }
}
