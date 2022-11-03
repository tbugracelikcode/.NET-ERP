using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrderLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrderLine
{
    public interface ISalesOrderLinesRepository : IEfCoreRepository<SalesOrderLines>
    {
    }
}
