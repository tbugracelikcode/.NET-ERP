using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;

namespace TsiErp.Business.Entities.Calendar.Services
{
    public interface ICalendarsAppService : ICrudAppService<SelectCalendarsDto, ListCalendarsDto, CreateCalendarsDto, UpdateCalendarsDto, ListCalendarsParameterDto>
    {
        Task<IDataResult<IList<SelectCalendarDaysDto>>> GetDaysListAsync(Guid calendarID);


        bool UpdateDays(SelectCalendarDaysDto day);

        Task<IDataResult<SelectCalendarsDto>> UpdateWithoutDaysAsync(UpdateCalendarsDto input);

        Task<IDataResult<SelectCalendarLinesDto>> GetLinebyStationDateAsync(Guid stationID, DateTime date);

    }
}
