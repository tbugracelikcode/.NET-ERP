using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchasePriceLine.Dtos;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    public interface IPurchasePricesAppService : ICrudAppService<SelectPurchasePricesDto, ListPurchasePricesDto, CreatePurchasePricesDto, UpdatePurchasePricesDto, ListPurchasePricesParameterDto>
    {
        Task<IDataResult<IList<SelectPurchasePriceLinesDto>>> GetSelectLineListAsync(Guid productId);
    }
}
