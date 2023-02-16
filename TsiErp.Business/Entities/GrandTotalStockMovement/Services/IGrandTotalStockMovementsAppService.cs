using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement.Dtos;

namespace TsiErp.Business.Entities.GrandTotalStockMovement.Services
{
    public interface IGrandTotalStockMovementsAppService : ICrudAppService<SelectGrandTotalStockMovementsDto, ListGrandTotalStockMovementsDto, CreateGrandTotalStockMovementsDto, UpdateGrandTotalStockMovementsDto, ListGrandTotalStockMovementsParameterDto>
    {
    }
}
