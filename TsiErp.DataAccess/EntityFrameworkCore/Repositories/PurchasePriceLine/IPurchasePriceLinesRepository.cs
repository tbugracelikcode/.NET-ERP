using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchasePriceLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePriceLine
{
    public interface IPurchasePriceLinesRepository : IEfCoreRepository<PurchasePriceLines>
    {
    }
}
