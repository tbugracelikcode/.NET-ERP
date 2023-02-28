using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesOrder.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    public interface ISalesOrdersAppService : ICrudAppService<SelectSalesOrderDto, ListSalesOrderDto, CreateSalesOrderDto, UpdateSalesOrderDto, ListSalesOrderParameterDto>
    {

        Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input);
    }
}
