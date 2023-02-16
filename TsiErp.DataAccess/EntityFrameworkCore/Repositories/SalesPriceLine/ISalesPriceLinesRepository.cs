using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesPriceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPriceLine
{
    public interface ISalesPriceLinesRepository : IEfCoreRepository<SalesPriceLines>
    {
    }
}
