using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.PurchasingUnsuitabilityItem.Dtos;
using TsiErp.Localizations.Resources.PurchasingUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IPurchasingUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class PurchasingUnsuitabilityItemsAppService : ApplicationService<PurchasingUnsuitabilityItemsResource>, IPurchasingUnsuitabilityItemsAppService
    {
        public PurchasingUnsuitabilityItemsAppService(IStringLocalizer<PurchasingUnsuitabilityItemsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> CreateAsync(CreatePurchasingUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListPurchasingUnsuitabilityItemsDto>>> GetListAsync(ListPurchasingUnsuitabilityItemsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> UpdateAsync(UpdatePurchasingUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchasingUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
