using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityItemsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreFinalControlUnsuitabilityItemsRepository : EfCoreRepository<FinalControlUnsuitabilityItems>, IFinalControlUnsuitabilityItemsRepository
    {
        public EFCoreFinalControlUnsuitabilityItemsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
