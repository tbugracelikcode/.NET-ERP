using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    public interface IPurchaseOrdersAppService : ICrudAppService<SelectPurchaseOrdersDto, ListPurchaseOrdersDto, CreatePurchaseOrdersDto, UpdatePurchaseOrdersDto, ListPurchaseOrdersParameterDto>
    {

        Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input);
    }
}
