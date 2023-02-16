using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesProposition;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder
{
    public interface ISalesOrdersRepository : IEfCoreRepository<SalesOrders>
    {
    }
}
