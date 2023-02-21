using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.UnplannedMaintenance.Dtos;

namespace TsiErp.Business.Entities.UnplannedMaintenance.Services
{
    public interface IUnplannedMaintenancesAppService : ICrudAppService<SelectUnplannedMaintenancesDto, ListUnplannedMaintenancesDto, CreateUnplannedMaintenancesDto, UpdateUnplannedMaintenancesDto, ListUnplannedMaintenancesParameterDto>
    {

    }
}
