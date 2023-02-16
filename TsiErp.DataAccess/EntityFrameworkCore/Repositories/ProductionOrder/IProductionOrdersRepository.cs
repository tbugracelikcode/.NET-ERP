using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductionOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder
{
    public interface IProductionOrdersRepository : IEfCoreRepository<ProductionOrders>
    {
    }
}
