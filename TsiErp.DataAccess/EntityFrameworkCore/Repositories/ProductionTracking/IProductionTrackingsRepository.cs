using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionTracking;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking
{
    public interface IProductionTrackingsRepository : IEfCoreRepository<ProductionTrackings>
    {
    }
}
