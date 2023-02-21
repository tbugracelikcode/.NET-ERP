using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    public interface IPurchaseOrdersAppService : ICrudAppService<SelectPurchaseOrdersDto, ListPurchaseOrdersDto, CreatePurchaseOrdersDto, UpdatePurchaseOrdersDto, ListPurchaseOrdersParameterDto>
    {

        Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input);
    }
}
