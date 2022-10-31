using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Shift.Dtos;

namespace TsiErp.Business.Entities.Shift.Services
{
    public interface IShiftsAppService : ICrudAppService<SelectShiftsDto, ListShiftsDto, CreateShiftsDto, UpdateShiftsDto, ListShiftsParameterDto>
    {
    }
}
