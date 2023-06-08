using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    public interface IShippingAdressesAppService : ICrudAppService<SelectShippingAdressesDto, ListShippingAdressesDto, CreateShippingAdressesDto, UpdateShippingAdressesDto, ListShippingAdressesParameterDto>
    {
    }
}
