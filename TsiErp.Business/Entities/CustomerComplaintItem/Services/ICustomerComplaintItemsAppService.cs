using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Services
{
    public interface ICustomerComplaintItemsAppService : ICrudAppService<SelectCustomerComplaintItemsDto, ListCustomerComplaintItemsDto, CreateCustomerComplaintItemsDto, UpdateCustomerComplaintItemsDto, ListCustomerComplaintItemsParameterDto>
    {
    }
}
