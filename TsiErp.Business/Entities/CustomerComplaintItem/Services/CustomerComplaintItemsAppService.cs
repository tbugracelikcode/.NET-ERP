using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;
using TsiErp.Localizations.Resources.CustomerComplaintItems.Page;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Services
{
    [ServiceRegistration(typeof(ICustomerComplaintItemsAppService), DependencyInjectionType.Scoped)]
    public class CustomerComplaintItemsAppService : ApplicationService<CustomerComplaintItemsResource>, ICustomerComplaintItemsAppService
    {
        public CustomerComplaintItemsAppService(IStringLocalizer<CustomerComplaintItemsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectCustomerComplaintItemsDto>> CreateAsync(CreateCustomerComplaintItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectCustomerComplaintItemsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListCustomerComplaintItemsDto>>> GetListAsync(ListCustomerComplaintItemsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectCustomerComplaintItemsDto>> UpdateAsync(UpdateCustomerComplaintItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectCustomerComplaintItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
