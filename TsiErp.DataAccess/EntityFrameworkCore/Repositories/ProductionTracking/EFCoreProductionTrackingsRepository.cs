using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.ProductionTracking;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking
{
    [ServiceRegistration(typeof(IProductionTrackingsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductionTrackingsRepository : EfCoreRepository<ProductionTrackings>, IProductionTrackingsRepository
    {
        public EFCoreProductionTrackingsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
