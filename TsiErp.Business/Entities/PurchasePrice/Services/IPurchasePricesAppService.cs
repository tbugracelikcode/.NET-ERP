using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchasePriceLine.Dtos;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    public interface IPurchasePricesAppService : ICrudAppService<SelectPurchasePricesDto, ListPurchasePricesDto, CreatePurchasePricesDto, UpdatePurchasePricesDto, ListPurchasePricesParameterDto>
    {
        Task<IDataResult<IList<SelectPurchasePriceLinesDto>>> GetSelectLineListAsync(Guid productId);
    }
}
