using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StationInventory.Dtos;

namespace TsiErp.Business.Entities.StationInventory.Services
{
    public interface IStationInventoriesAppService : ICrudAppService<SelectStationInventoriesDto, ListStationInventoriesDto, CreateStationInventoriesDto, UpdateStationInventoriesDto, ListStationInventoriesParameterDto>

    {
    }
}
