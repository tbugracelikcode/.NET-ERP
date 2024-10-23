using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;

namespace TsiErp.Business.Entities.PurchaseManagement.PurchaseInvoice.Services
{
    public interface IPurchaseInvoicesAppService : ICrudAppService<SelectPurchaseInvoicesDto, ListPurchaseInvoicesDto, CreatePurchaseInvoicesDto, UpdatePurchaseInvoicesDto, ListPurchaseInvoicesParameterDto>
    {
        Task<IDataResult<IList<SelectPurchaseInvoiceLinesDto>>> GetLineListAsync();
    }
}
