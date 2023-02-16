using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.MaintenanceInstruction.Services
{
    public interface IMaintenanceInstructionsAppService : ICrudAppService<SelectMaintenanceInstructionsDto, ListMaintenanceInstructionsDto, CreateMaintenanceInstructionsDto, UpdateMaintenanceInstructionsDto, ListMaintenanceInstructionsParameterDto>
    {
        Task<IDataResult<SelectMaintenanceInstructionsDto>> GetbyPeriodStationAsync(Guid? stationID, Guid? periodID);
    }
}
