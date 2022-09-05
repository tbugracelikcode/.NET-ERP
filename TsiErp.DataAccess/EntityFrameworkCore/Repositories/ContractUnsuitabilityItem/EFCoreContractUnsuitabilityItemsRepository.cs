using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem
{
    [ServiceRegistration(typeof(IContractUnsuitabilityItemsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreContractUnsuitabilityItemsRepository : EfCoreRepository<ContractUnsuitabilityItems>, IContractUnsuitabilityItemsRepository
    {
        public EFCoreContractUnsuitabilityItemsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
