
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord.Dtos;

namespace TsiErp.Business.Entities.CostManagement.StandartStationCostRecord.Services
{
    public interface IStandartStationCostRecordsAppService : ICrudAppService<SelectStandartStationCostRecordsDto, ListStandartStationCostRecordsDto, CreateStandartStationCostRecordsDto, UpdateStandartStationCostRecordsDto, ListStandartStationCostRecordsParameterDto>
    {
    }
}
