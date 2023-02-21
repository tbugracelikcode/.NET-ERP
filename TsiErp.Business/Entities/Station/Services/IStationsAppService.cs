using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Station.Dtos;

namespace TsiErp.Business.Entities.Station.Services
{
    public interface IStationsAppService : ICrudAppService<SelectStationsDto, ListStationsDto, CreateStationsDto, UpdateStationsDto, ListStationsParameterDto>
    {
    }
}
