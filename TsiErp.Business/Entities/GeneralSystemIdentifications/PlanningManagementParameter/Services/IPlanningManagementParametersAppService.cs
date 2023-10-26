using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Services
{
    public interface IPlanningManagementParametersAppService : ICrudAppService<SelectPlanningManagementParametersDto, ListPlanningManagementParametersDto, CreatePlanningManagementParametersDto, UpdatePlanningManagementParametersDto, ListPlanningManagementParametersParameterDto>
    {
        Task<IDataResult<SelectPlanningManagementParametersDto>> GetPlanningManagementParametersAsync();
    }
}