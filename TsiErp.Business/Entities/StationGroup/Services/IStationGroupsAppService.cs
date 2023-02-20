using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    public interface IStationGroupsAppService : ICrudAppService<SelectStationGroupsDto, ListStationGroupsDto, CreateStationGroupsDto, UpdateStationGroupsDto, ListStationGroupsParameterDto>
    {
    }
}
