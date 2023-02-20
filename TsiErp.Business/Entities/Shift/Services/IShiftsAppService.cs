using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Shift.Dtos;

namespace TsiErp.Business.Entities.Shift.Services
{
    public interface IShiftsAppService : ICrudAppService<SelectShiftsDto, ListShiftsDto, CreateShiftsDto, UpdateShiftsDto, ListShiftsParameterDto>
    {
    }
}
