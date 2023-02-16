using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.ForecastLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ForecastLine
{
    [ServiceRegistration(typeof(IForecastLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreForecastLinesRepository : EfCoreRepository<ForecastLines>, IForecastLinesRepository
    {
        public EFCoreForecastLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
