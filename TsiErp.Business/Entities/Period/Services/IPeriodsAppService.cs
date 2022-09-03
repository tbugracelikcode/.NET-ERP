using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Services
{
    public interface IPeriodsAppService : ICrudAppService<Periods, SelectPeriodsDto, ListPeriodsDto, CreatePeriodsDto, UpdatePeriodsDto, ListPeriodsParameterDto>
    {
    }
}
