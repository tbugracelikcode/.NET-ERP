using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Calendar;
using TsiErp.Entities.Entities.Calendar;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Calendar
{
    [ServiceRegistration(typeof(ICalendarsRepository), DependencyInjectionType.Scoped)]

    public class EFCoreCalendarsRepository : EfCoreRepository<Calendars>, ICalendarsRepository
    {
        public EFCoreCalendarsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
