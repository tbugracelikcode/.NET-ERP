using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrderChangeItem
{
    public interface IProductionOrderChangeItemsRepository : IEfCoreRepository<ProductionOrderChangeItems>
    {
    }
}
