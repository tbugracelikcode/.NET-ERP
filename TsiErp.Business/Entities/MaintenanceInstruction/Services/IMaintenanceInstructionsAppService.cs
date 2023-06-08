using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Services
{
    public interface IMaintenanceInstructionsAppService : ICrudAppService<SelectMaintenanceInstructionsDto, ListMaintenanceInstructionsDto, CreateMaintenanceInstructionsDto, UpdateMaintenanceInstructionsDto, ListMaintenanceInstructionsParameterDto>
    {
        Task<IDataResult<SelectMaintenanceInstructionsDto>> GetbyPeriodStationAsync(Guid? stationID, Guid? periodID);
    }
}
