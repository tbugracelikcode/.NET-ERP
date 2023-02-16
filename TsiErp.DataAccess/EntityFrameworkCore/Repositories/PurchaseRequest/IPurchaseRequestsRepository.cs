using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.PurchaseRequest;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequest
{
    public interface IPurchaseRequestsRepository : IEfCoreRepository<PurchaseRequests>
    {
    }
}
