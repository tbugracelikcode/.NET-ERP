using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    public interface IPurchaseOrdersAppService : ICrudAppService<SelectPurchaseOrdersDto, ListPurchaseOrdersDto, CreatePurchaseOrdersDto, UpdatePurchaseOrdersDto, ListPurchaseOrdersParameterDto>
    {

        Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input);

        Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderMRPAsync(CreatePurchaseOrdersDto input);

        Task<IDataResult<IList<SelectPurchaseOrderLinesDto>>> GetLineListAsync();

        Task<IDataResult<IList<ListPurchaseOrdersDto>>> GetQualityControlPendingListAsync();

        Task<IDataResult<IList<SelectPurchaseOrdersDto>>> GetQualityControlSelectListAsync();

        Task<IDataResult<SelectPurchaseOrdersDto>> UpdateApproveOrderAsync(UpdatePurchaseOrdersDto input);

        Task<IDataResult<SelectPurchaseOrdersDto>> UpdateApproveBillAsync(UpdatePurchaseOrdersDto input);

        Task<IDataResult<SelectPurchaseOrdersDto>> UpdateApproveWayBillAsync(UpdatePurchaseOrdersDto input);

        Task<IDataResult<SelectPurchaseOrdersDto>> UpdateCancelOrderAsync(UpdatePurchaseOrdersDto input);

        Task<IDataResult<SelectPurchaseOrdersDto>> UpdateOrderCreateStockFichesAsync(UpdatePurchaseOrdersDto input);

    }
}
