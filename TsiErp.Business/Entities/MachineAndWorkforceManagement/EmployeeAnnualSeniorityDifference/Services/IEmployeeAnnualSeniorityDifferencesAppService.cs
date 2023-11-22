using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;

namespace TsiErp.Business.Entities.EmployeeAnnualSeniorityDifference.Services
{
    public interface IEmployeeAnnualSeniorityDifferencesAppService : ICrudAppService<SelectEmployeeAnnualSeniorityDifferencesDto, ListEmployeeAnnualSeniorityDifferencesDto, CreateEmployeeAnnualSeniorityDifferencesDto, UpdateEmployeeAnnualSeniorityDifferencesDto, ListEmployeeAnnualSeniorityDifferencesParameterDto>
    {
    }
}
