using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period
{
    public class EFCorePeriodsRepository : EfCoreRepository<TsiErpDbContext, Periods>, IPeriodsRepository
    {
    }
}
