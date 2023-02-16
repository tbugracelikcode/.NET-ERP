using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.ExchangeRate;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ExchangeRate
{
    [ServiceRegistration(typeof(IExchangeRatesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreExchangeRatesRepository : EfCoreRepository<ExchangeRates>, IExchangeRatesRepository
    {
        public EFCoreExchangeRatesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
