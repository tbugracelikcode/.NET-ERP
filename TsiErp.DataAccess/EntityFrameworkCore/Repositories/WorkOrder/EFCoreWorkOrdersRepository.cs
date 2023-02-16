using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.WorkOrder
{
    [ServiceRegistration(typeof(IWorkOrdersRepository), DependencyInjectionType.Scoped)]
    public class EFCoreWorkOrdersRepository : EfCoreRepository<WorkOrders>, IWorkOrdersRepository
    {
        public EFCoreWorkOrdersRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
