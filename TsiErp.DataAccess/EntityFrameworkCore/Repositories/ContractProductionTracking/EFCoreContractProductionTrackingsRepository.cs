using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.ContractProductionTracking;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractProductionTracking
{
    [ServiceRegistration(typeof(IContractProductionTrackingsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreContractProductionTrackingsRepository : EfCoreRepository<ContractProductionTrackings>, IContractProductionTrackingsRepository
    {
        public EFCoreContractProductionTrackingsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
