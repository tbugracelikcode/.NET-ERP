using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.StationInventory;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationInventory
{
    [ServiceRegistration(typeof(IStationInventoriesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreStationInventoriesRepository : EfCoreRepository<StationInventories>, IStationInventoriesRepository
    {
        public EFCoreStationInventoriesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
