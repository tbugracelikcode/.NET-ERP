using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    public interface ISalesOrdersAppService : ICrudAppService<SelectSalesOrderDto, ListSalesOrderDto, CreateSalesOrderDto, UpdateSalesOrderDto, ListSalesOrderParameterDto>
    {

        Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input);

        Task<IDataResult<IList<SelectSalesOrderLinesDto>>> GetLineSelectListAsync(); 
        Task<IDataResult<IList<SelectSalesOrderLinesDto>>> ODGetLineOrderstListAsync();

        Task<IDataResult<SelectSalesOrderDto>> UpdateOrderApprovalAsync(UpdateSalesOrderDto input);
        Task<IDataResult<SelectSalesOrderDto>> UpdatePendingAsync(UpdateSalesOrderDto input);
        Task<IDataResult<SelectSalesOrderDto>> UpdateGiveProductionAsync(UpdateSalesOrderDto input);

        decimal GetLastOrderPriceByCurrentAccountProduct(Guid CurrentAccountID, Guid ProductID);
    }
}
