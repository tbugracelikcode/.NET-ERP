using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    public interface ISalesOrdersAppService : ICrudAppService<SelectSalesOrderDto, ListSalesOrderDto, CreateSalesOrderDto, UpdateSalesOrderDto, ListSalesOrderParameterDto>
    {

        Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input);
    }
}
