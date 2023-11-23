using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Services
{
    public interface IGeneralSkillRecordPrioritiesAppService : ICrudAppService<SelectGeneralSkillRecordPrioritiesDto, ListGeneralSkillRecordPrioritiesDto, CreateGeneralSkillRecordPrioritiesDto, UpdateGeneralSkillRecordPrioritiesDto, ListGeneralSkillRecordPrioritiesParameterDto>
    {
    }
}
