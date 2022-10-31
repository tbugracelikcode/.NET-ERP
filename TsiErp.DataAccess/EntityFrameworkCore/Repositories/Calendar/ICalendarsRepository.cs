using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Calendar;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Calendar
{
    public interface ICalendarsRepository : IEfCoreRepository<Calendars>
    {
    }
}
