using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Services
{
    public interface IFinalControlUnsuitabilityItemsAppService : ICrudAppService<SelectFinalControlUnsuitabilityItemsDto, ListFinalControlUnsuitabilityItemsDto, CreateFinalControlUnsuitabilityItemsDto, UpdateFinalControlUnsuitabilityItemsDto, ListFinalControlUnsuitabilityItemsParameterDto>
    {
    }
}
