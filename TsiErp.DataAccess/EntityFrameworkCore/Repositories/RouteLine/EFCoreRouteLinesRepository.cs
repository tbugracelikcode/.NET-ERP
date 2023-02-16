using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.RouteLine;
using TsiErp.Entities.Entities.RouteLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.RouteLine
{
    [ServiceRegistration(typeof(IRouteLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreRouteLinesRepository : EfCoreRepository<RouteLines>, IRouteLinesRepository
    {
        public EFCoreRouteLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
