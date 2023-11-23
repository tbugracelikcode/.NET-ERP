using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;

namespace TsiErp.Business.Entities.EmployeeSeniority.Services
{
    public interface IEmployeeSenioritiesAppService : ICrudAppService<SelectEmployeeSenioritiesDto, ListEmployeeSenioritiesDto, CreateEmployeeSenioritiesDto, UpdateEmployeeSenioritiesDto, ListEmployeeSenioritiesParameterDto>
    {
    }
}
