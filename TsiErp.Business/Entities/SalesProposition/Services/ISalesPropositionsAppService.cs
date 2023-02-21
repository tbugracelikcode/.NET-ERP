using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    public interface ISalesPropositionsAppService : ICrudAppService<SelectSalesPropositionsDto, ListSalesPropositionsDto, CreateSalesPropositionsDto, UpdateSalesPropositionsDto, ListSalesPropositionsParamaterDto>
    {
        Task UpdateSalesPropositionLineState(List<SelectSalesOrderLinesDto> orderLineList, SalesPropositionLineStateEnum lineState);
    }
}
