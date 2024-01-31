using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    public interface ISalesOrdersAppService : ICrudAppService<SelectSalesOrderDto, ListSalesOrderDto, CreateSalesOrderDto, UpdateSalesOrderDto, ListSalesOrderParameterDto>
    {

        Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input);

        Task<IDataResult<IList<SelectSalesOrderLinesDto>>> GetLineSelectListAsync();
    }
}
