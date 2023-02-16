using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;
using Tsi.Core.Services.BusinessCoreServices;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    public interface IShippingAdressesAppService : ICrudAppService<SelectShippingAdressesDto, ListShippingAdressesDto, CreateShippingAdressesDto, UpdateShippingAdressesDto, ListShippingAdressesParameterDto>
    {
    }
}
