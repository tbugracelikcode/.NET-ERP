using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem.Dtos;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.Services
{
    public interface IProductionOrderChangeItemsAppService : ICrudAppService<SelectProductionOrderChangeItemsDto, ListProductionOrderChangeItemsDto, CreateProductionOrderChangeItemsDto, UpdateProductionOrderChangeItemsDto, ListProductionOrderChangeItemsParameterDto>
    {
    }
}
