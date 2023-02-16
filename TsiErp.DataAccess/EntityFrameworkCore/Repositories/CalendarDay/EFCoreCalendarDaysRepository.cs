using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.CalendarDay;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarDay
{
    [ServiceRegistration(typeof(ICalendarDaysRepository), DependencyInjectionType.Scoped)]

    public class EFCoreCalendarDaysRepository : EfCoreRepository<CalendarDays>, ICalendarDaysRepository
    {
        public EFCoreCalendarDaysRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
