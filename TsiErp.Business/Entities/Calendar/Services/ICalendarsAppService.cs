using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.CalendarLine.Dtos;

namespace TsiErp.Business.Entities.Calendar.Services
{
    public interface ICalendarsAppService : ICrudAppService<SelectCalendarsDto, ListCalendarsDto, CreateCalendarsDto, UpdateCalendarsDto, ListCalendarsParameterDto>
    {
        Task<IDataResult<IList<SelectCalendarDaysDto>>> GetDaysListAsync(Guid calendarID);

        Task<IDataResult<IList<ListCalendarLinesDto>>> GetLineListAsync(Guid calendarID);
    }
}
