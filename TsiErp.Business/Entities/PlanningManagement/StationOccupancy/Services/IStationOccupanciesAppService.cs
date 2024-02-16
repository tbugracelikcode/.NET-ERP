using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PlanningManagement.StationOccupancy.Dtos;

namespace TsiErp.Business.Entities.StationOccupancy.Services
{
    public interface IStationOccupanciesAppService : ICrudAppService<SelectStationOccupanciesDto, ListStationOccupanciesDto, CreateStationOccupanciesDto, UpdateStationOccupanciesDto, ListStationOccupanciesParameterDto>

    {
    }
}
