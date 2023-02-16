using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Shift.Dtos;

namespace TsiErp.Business.Entities.Shift.Services
{
    public interface IShiftsAppService : ICrudAppService<SelectShiftsDto, ListShiftsDto, CreateShiftsDto, UpdateShiftsDto, ListShiftsParameterDto>
    {
    }
}
