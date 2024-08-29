using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    public interface IPurchasePricesAppService : ICrudAppService<SelectPurchasePricesDto, ListPurchasePricesDto, CreatePurchasePricesDto, UpdatePurchasePricesDto, ListPurchasePricesParameterDto>
    {
        Task<IDataResult<IList<SelectPurchasePriceLinesDto>>> GetSelectLineListAsync(Guid productId);

        Task<IDataResult<SelectPurchasePriceLinesDto>> GetDefinedProductPriceAsync(Guid productId, Guid currentAccountCardId, Guid currencyId, bool isApproved, DateTime date);
    }
}
