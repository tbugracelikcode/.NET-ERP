using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;

namespace TsiErp.Business.Entities.MaintenancePeriod.Services
{
    public interface IMaintenancePeriodsAppService : ICrudAppService<SelectMaintenancePeriodsDto, ListMaintenancePeriodsDto, CreateMaintenancePeriodsDto, UpdateMaintenancePeriodsDto, ListMaintenancePeriodsParameterDto>
    {
    }
}
