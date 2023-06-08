using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityItem.Dtos;
using TsiErp.Localizations.Resources.OperationUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityItemsAppService : ApplicationService<OperationUnsuitabilityItemsResource>, IOperationUnsuitabilityItemsAppService
    {
        public OperationUnsuitabilityItemsAppService(IStringLocalizer<OperationUnsuitabilityItemsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> CreateAsync(CreateOperationUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListOperationUnsuitabilityItemsDto>>> GetListAsync(ListOperationUnsuitabilityItemsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> UpdateAsync(UpdateOperationUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
