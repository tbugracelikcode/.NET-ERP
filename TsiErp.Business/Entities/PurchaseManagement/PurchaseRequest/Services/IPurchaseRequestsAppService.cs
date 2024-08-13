using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.PurchaseRequest.Services
{
    public interface IPurchaseRequestsAppService : ICrudAppService<SelectPurchaseRequestsDto, ListPurchaseRequestsDto, CreatePurchaseRequestsDto, UpdatePurchaseRequestsDto, ListPurchaseRequestsParameterDto>
    {
        Task UpdatePurchaseRequestLineState(List<SelectPurchaseOrderLinesDto> orderLineList, PurchaseRequestLineStateEnum lineState);

        Task<IDataResult<IList<SelectPurchaseRequestLinesDto>>> GetLineListAsync();

        Task<IDataResult<SelectPurchaseRequestsDto>> ConvertToPurchaseRequestMRPAsync(CreatePurchaseRequestsDto input);
    }
}
