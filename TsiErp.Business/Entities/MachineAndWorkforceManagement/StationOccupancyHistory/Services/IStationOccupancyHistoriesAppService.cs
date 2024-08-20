using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Services
{
    public interface IStationOccupancyHistoriesAppService : ICrudAppService<SelectStationOccupancyHistoriesDto, ListStationOccupancyHistoriesDto, CreateStationOccupancyHistoriesDto, UpdateStationOccupancyHistoriesDto, ListStationOccupancyHistoriesParameterDto>
    {
    }
}
