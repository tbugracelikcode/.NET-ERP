using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.EducationLevelScore.Services
{
    public interface IEducationLevelScoresAppService : ICrudAppService<SelectEducationLevelScoresDto, ListEducationLevelScoresDto, CreateEducationLevelScoresDto, UpdateEducationLevelScoresDto, ListEducationLevelScoresParameterDto>
    {
    }
}
