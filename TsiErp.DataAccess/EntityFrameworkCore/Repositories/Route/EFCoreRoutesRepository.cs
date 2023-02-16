using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route;
using TsiErp.Entities.Entities.Route;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route
{
    [ServiceRegistration(typeof(IRoutesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreRoutesRepository : EfCoreRepository<Routes>, IRoutesRepository
    {
        public EFCoreRoutesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
