using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos;

namespace TsiErp.Business.Entities.EmployeeScoring.Services
{
    public interface IEmployeeScoringsAppService : ICrudAppService<SelectEmployeeScoringsDto, ListEmployeeScoringsDto, CreateEmployeeScoringsDto, UpdateEmployeeScoringsDto, ListEmployeeScoringsParameterDto>
    {
    }
}
