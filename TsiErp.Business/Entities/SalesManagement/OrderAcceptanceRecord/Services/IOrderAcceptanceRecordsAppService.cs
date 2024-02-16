using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;

namespace TsiErp.Business.Entities.OrderAcceptanceRecord.Services
{
    public interface IOrderAcceptanceRecordsAppService : ICrudAppService<SelectOrderAcceptanceRecordsDto, ListOrderAcceptanceRecordsDto, CreateOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto, ListOrderAcceptanceRecordsParameterDto>
    {
        Task<IDataResult<SelectOrderAcceptanceRecordLinesDto>> UpdateLineAsync(Guid lineID, DateTime supplyDate);
    }
}
