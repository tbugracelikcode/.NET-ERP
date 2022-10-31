using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Calendar.Dtos;

namespace TsiErp.Business.Entities.Calendar.Services
{
    public interface ICalendarsAppService : ICrudAppService<SelectCalendarsDto, ListCalendarsDto, CreateCalendarsDto, UpdateCalendarsDto, ListCalendarsParameterDto>
    {
    }
}
