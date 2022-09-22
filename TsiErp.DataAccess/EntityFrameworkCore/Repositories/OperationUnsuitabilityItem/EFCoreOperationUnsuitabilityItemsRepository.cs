using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityItemsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreOperationUnsuitabilityItemsRepository : EfCoreRepository<OperationUnsuitabilityItems>, IOperationUnsuitabilityItemsRepository
    {
        public EFCoreOperationUnsuitabilityItemsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
