using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.PurchasingUnsuitabilityItem.Services
{
    public interface IPurchasingUnsuitabilityItemsAppService : ICrudAppService<SelectPurchasingUnsuitabilityItemsDto, ListPurchasingUnsuitabilityItemsDto, CreatePurchasingUnsuitabilityItemsDto, UpdatePurchasingUnsuitabilityItemsDto, ListPurchasingUnsuitabilityItemsParameterDto>
    {
    }
}
