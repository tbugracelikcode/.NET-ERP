using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.ProductionTracking;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason
{
    [ServiceRegistration(typeof(IHaltReasonsRepository), DependencyInjectionType.Scoped)]

    public class EFCoreHaltReasonsRepository : EfCoreRepository<HaltReasons>, IHaltReasonsRepository
    {
        public EFCoreHaltReasonsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
