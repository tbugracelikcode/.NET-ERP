using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    public interface ISalesPropositionsAppService : ICrudAppService<SelectSalesPropositionsDto, ListSalesPropositionsDto, CreateSalesPropositionsDto, UpdateSalesPropositionsDto, ListSalesPropositionsParamaterDto>
    {
        Task UpdateSalesPropositionLineState(List<SelectSalesOrderLinesDto> orderLineList, SalesPropositionLineStateEnum lineState);
    }
}
