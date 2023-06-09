using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityItem.Dtos;
using TsiErp.Localizations.Resources.FinalControlUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class FinalControlUnsuitabilityItemsAppService : ApplicationService<FinalControlUnsuitabilityItemsResource>, IFinalControlUnsuitabilityItemsAppService
    {
        public FinalControlUnsuitabilityItemsAppService(IStringLocalizer<FinalControlUnsuitabilityItemsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> CreateAsync(CreateFinalControlUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListFinalControlUnsuitabilityItemsDto>>> GetListAsync(ListFinalControlUnsuitabilityItemsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> UpdateAsync(UpdateFinalControlUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
