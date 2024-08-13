using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;

namespace TsiErp.Business.Entities.OrderAcceptanceRecord.Services
{
    public interface IOrderAcceptanceRecordsAppService : ICrudAppService<SelectOrderAcceptanceRecordsDto, ListOrderAcceptanceRecordsDto, CreateOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto, ListOrderAcceptanceRecordsParameterDto>
    {
        Task<IDataResult<SelectOrderAcceptanceRecordLinesDto>> UpdateLineAsync(Guid lineID, DateTime supplyDate);

        Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateTechApprovalAsync(UpdateOrderAcceptanceRecordsDto input);

        Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateOrderApprovalAsync(UpdateOrderAcceptanceRecordsDto input);

        Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdatePendingAsync(UpdateOrderAcceptanceRecordsDto input);

        Task<IDataResult<SelectOrderAcceptanceRecordsDto>> UpdateAcceptanceOrderAsync(UpdateOrderAcceptanceRecordsDto input);
    }
}
