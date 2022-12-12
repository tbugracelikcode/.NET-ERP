using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchaseRequestLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequestLine
{
    public interface IPurchaseRequestLinesRepository : IEfCoreRepository<PurchaseRequestLines>
    {
    }
}
