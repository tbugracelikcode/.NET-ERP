using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarLine;
using TsiErp.Entities.Entities.CalendarLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarLine
{
    [ServiceRegistration(typeof(ICalendarLinesRepository), DependencyInjectionType.Scoped)]

    public class EFCoreCalendarLinesRepository : EfCoreRepository<CalendarLines>, ICalendarLinesRepository
    {
        public EFCoreCalendarLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
