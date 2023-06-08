using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    public interface IPurchaseOrdersAppService : ICrudAppService<SelectPurchaseOrdersDto, ListPurchaseOrdersDto, CreatePurchaseOrdersDto, UpdatePurchaseOrdersDto, ListPurchaseOrdersParameterDto>
    {

        Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input);
    }
}
