using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.ShippingManagementParameter.Services
{
    public interface IShippingManagementParametersAppService : ICrudAppService<SelectShippingManagementParametersDto, ListShippingManagementParametersDto, CreateShippingManagementParametersDto, UpdateShippingManagementParametersDto, ListShippingManagementParametersParameterDto>
    {
    }
}