using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Forecast;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Forecast
{
    [ServiceRegistration(typeof(IForecastsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreForecastsRepository : EfCoreRepository<Forecasts>, IForecastsRepository
    {
        public EFCoreForecastsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
