using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseOrderLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrderLine
{
    public interface IPurchaseOrderLinesRepository : IEfCoreRepository<PurchaseOrderLines>
    {
    }
}
