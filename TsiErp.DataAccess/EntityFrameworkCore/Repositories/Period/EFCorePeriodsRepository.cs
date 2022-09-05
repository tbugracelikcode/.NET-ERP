using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period
{
    [ServiceRegistration(typeof(IPeriodsRepository), DependencyInjectionType.Scoped)]
    public class EFCorePeriodsRepository : EfCoreRepository<Periods>, IPeriodsRepository
    {
        public EFCorePeriodsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
