using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.ProductionDateReferenceNumber.Services
{
    public interface IProductionDateReferenceNumbersAppService : ICrudAppService<SelectProductionDateReferenceNumbersDto, ListProductionDateReferenceNumbersDto, CreateProductionDateReferenceNumbersDto, UpdateProductionDateReferenceNumbersDto, ListProductionDateReferenceNumbersParameterDto>
    {
    }
}
