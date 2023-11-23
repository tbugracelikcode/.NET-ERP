using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Services
{
    public interface IEmployeeGeneralSkillRecordsAppService : ICrudAppService<SelectEmployeeGeneralSkillRecordsDto, ListEmployeeGeneralSkillRecordsDto, CreateEmployeeGeneralSkillRecordsDto, UpdateEmployeeGeneralSkillRecordsDto, ListEmployeeGeneralSkillRecordsParameterDto>
    {
    }
}
