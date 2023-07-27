using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter.Services
{
    public interface IMaintenanceManagementParametersAppService : ICrudAppService<SelectMaintenanceManagementParametersDto, ListMaintenanceManagementParametersDto, CreateMaintenanceManagementParametersDto, UpdateMaintenanceManagementParametersDto, ListMaintenanceManagementParametersParameterDto>
    {
    }
}