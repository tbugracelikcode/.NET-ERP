using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTrackingHaltLine
{
    [ServiceRegistration(typeof(IProductionTrackingHaltLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductionTrackingHaltLinesRepository : EfCoreRepository<ProductionTrackingHaltLines>, IProductionTrackingHaltLinesRepository
    {
        public EFCoreProductionTrackingHaltLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }

    }
}
