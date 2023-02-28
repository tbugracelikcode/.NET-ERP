using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
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
