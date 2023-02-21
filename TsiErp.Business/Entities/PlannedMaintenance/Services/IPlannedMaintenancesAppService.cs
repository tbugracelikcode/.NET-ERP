using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;

namespace TsiErp.Business.Entities.PlannedMaintenance.Services
{
    public interface IPlannedMaintenancesAppService : ICrudAppService<SelectPlannedMaintenancesDto, ListPlannedMaintenancesDto, CreatePlannedMaintenancesDto, UpdatePlannedMaintenancesDto, ListPlannedMaintenancesParameterDto>
    {

    }
}
