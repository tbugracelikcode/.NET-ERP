using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;

namespace TsiErp.Business.Entities.MaintenanceMRP.Services
{
    public interface IMaintenanceMRPsAppService : ICrudAppService<SelectMaintenanceMRPsDto, ListMaintenanceMRPsDto, CreateMaintenanceMRPsDto, UpdateMaintenanceMRPsDto, ListMaintenanceMRPsParameterDto>
    {
    }
}
