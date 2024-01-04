using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;

namespace TsiErp.Business.Entities.OrderAcceptanceRecord.Services
{
    public interface IOrderAcceptanceRecordsAppService : ICrudAppService<SelectOrderAcceptanceRecordsDto, ListOrderAcceptanceRecordsDto, CreateOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto, ListOrderAcceptanceRecordsParameterDto>
    {

    }
}
