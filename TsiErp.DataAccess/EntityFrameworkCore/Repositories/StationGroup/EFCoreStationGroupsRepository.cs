using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationGroup
{
    [ServiceRegistration(typeof(IStationGroupsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreStationGroupsRepository : EfCoreRepository<StationGroups>, IStationGroupsRepository
    {
        public EFCoreStationGroupsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
