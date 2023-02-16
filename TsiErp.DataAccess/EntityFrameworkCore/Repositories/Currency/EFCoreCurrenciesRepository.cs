using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.Currency;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Currency
{
    [ServiceRegistration(typeof(ICurrenciesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreCurrenciesRepository :  EfCoreRepository<Currencies>, ICurrenciesRepository
    {
        public EFCoreCurrenciesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
