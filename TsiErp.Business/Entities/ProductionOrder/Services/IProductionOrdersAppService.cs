using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    public interface IProductionOrdersAppService : ICrudAppService<SelectProductionOrdersDto, ListProductionOrdersDto, CreateProductionOrdersDto, UpdateProductionOrdersDto, ListProductionOrdersParameterDto>
    {
    }
}
