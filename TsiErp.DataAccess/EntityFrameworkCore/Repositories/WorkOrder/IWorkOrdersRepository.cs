using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.WorkOrder
{
    public interface IWorkOrdersRepository : IEfCoreRepository<WorkOrders>
    {
    }
}
