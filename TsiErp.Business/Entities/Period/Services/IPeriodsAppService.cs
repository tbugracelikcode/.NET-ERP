using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Services
{
    public interface IPeriodsAppService : ICrudAppService<SelectPeriodsDto, ListPeriodsDto, CreatePeriodsDto, UpdatePeriodsDto, ListPeriodsParameterDto>
    {
    }
}
