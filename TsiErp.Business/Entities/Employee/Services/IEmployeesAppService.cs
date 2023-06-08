using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;

namespace TsiErp.Business.Entities.Employee.Services
{
    public interface IEmployeesAppService : ICrudAppService<SelectEmployeesDto, ListEmployeesDto, CreateEmployeesDto, UpdateEmployeesDto, ListEmployeesParameterDto>
    {
    }
}
