using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Localizations.Resources.ContractUnsuitabilityItems.Page;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IContractUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class ContractUnsuitabilityItemsAppService : ApplicationService<ContractUnsuitabilityItemsResource>, IContractUnsuitabilityItemsAppService
    {
        public ContractUnsuitabilityItemsAppService(IStringLocalizer<ContractUnsuitabilityItemsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectContractUnsuitabilityItemsDto>> CreateAsync(CreateContractUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectContractUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListContractUnsuitabilityItemsDto>>> GetListAsync(ListContractUnsuitabilityItemsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateAsync(UpdateContractUnsuitabilityItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectContractUnsuitabilityItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
