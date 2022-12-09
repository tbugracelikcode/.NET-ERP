using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;

namespace TsiErp.Business.Entities.PurchaseRequest.Services
{
    public interface IPurchaseRequestsAppService : ICrudAppService<SelectPurchaseRequestsDto, ListPurchaseRequestsDto, CreatePurchaseRequestsDto, UpdatePurchaseRequestsDto, ListPurchaseRequestsParameterDto>
    {
        Task UpdatePurchaseRequestLineState(List<SelectPurchaseOrderLinesDto> orderLineList, PurchaseRequestLineStateEnum lineState);
    }
}
