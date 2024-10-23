using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;

namespace TsiErp.Business.Entities.Station.Services
{
    public interface IStationsAppService : ICrudAppService<SelectStationsDto, ListStationsDto, CreateStationsDto, UpdateStationsDto, ListStationsParameterDto>
    {
        Task<IDataResult<SelectStationsDto>> UpdateStationWorkStateAsync(Guid stationId, int workState);

    }
}
