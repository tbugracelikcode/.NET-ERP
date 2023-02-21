using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Services
{
    public interface IPurchasingUnsuitabilityItemsAppService : ICrudAppService<SelectPurchasingUnsuitabilityItemsDto, ListPurchasingUnsuitabilityItemsDto, CreatePurchasingUnsuitabilityItemsDto, UpdatePurchasingUnsuitabilityItemsDto, ListPurchasingUnsuitabilityItemsParameterDto>
    {
    }
}
