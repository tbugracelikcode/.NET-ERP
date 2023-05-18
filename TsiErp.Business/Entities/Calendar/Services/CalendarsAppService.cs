using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.CalendarLine.Dtos;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Calendar.Services
{
    [ServiceRegistration(typeof(ICalendarsAppService), DependencyInjectionType.Scoped)]
    public class CalendarsAppService : ApplicationService<BranchesResource>, ICalendarsAppService
    {
        public CalendarsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectCalendarsDto>> CreateAsync(CreateCalendarsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectCalendarsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<SelectCalendarDaysDto>>> GetDaysListAsync(Guid calendarID)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListCalendarLinesDto>>> GetLineListAsync(Guid calendarID)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListCalendarsDto>>> GetListAsync(ListCalendarsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectCalendarsDto>> UpdateAsync(UpdateCalendarsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectCalendarsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
