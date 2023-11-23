using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.TaskScoring.Services
{
    public interface ITaskScoringsAppService : ICrudAppService<SelectTaskScoringsDto, ListTaskScoringsDto, CreateTaskScoringsDto, UpdateTaskScoringsDto, ListTaskScoringsParameterDto>
    {
    }
}
