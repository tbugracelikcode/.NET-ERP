
using Microsoft.EntityFrameworkCore;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Station;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station
{
    [ServiceRegistration(typeof(IStationsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreStationsRepository : EfCoreRepository<Stations>, IStationsRepository
    {
        public EFCoreStationsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
