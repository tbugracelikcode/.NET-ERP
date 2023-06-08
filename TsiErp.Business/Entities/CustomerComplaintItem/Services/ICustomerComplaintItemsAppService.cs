using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Services
{
    public interface ICustomerComplaintItemsAppService : ICrudAppService<SelectCustomerComplaintItemsDto, ListCustomerComplaintItemsDto, CreateCustomerComplaintItemsDto, UpdateCustomerComplaintItemsDto, ListCustomerComplaintItemsParameterDto>
    {
    }
}
