using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrder
{
    public interface IPurchaseOrdersRepository : IEfCoreRepository<PurchaseOrders>
    {
    }
}
