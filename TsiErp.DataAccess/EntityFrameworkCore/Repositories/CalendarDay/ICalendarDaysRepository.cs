using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.CalendarDay;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalendarDay
{
    internal interface ICalendarDaysRepository : IEfCoreRepository<CalendarDays>
    {
    }
}
