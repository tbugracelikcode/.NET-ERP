using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.PurchasePrice.Dtos;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    public interface IPurchasePricesAppService : ICrudAppService<SelectPurchasePricesDto, ListPurchasePricesDto, CreatePurchasePricesDto, UpdatePurchasePricesDto, ListPurchasePricesParameterDto>
    {

    }
}
