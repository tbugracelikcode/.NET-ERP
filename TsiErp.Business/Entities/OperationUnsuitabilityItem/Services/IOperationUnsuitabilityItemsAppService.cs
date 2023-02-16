using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Services
{
    public interface IOperationUnsuitabilityItemsAppService : ICrudAppService<SelectOperationUnsuitabilityItemsDto, ListOperationUnsuitabilityItemsDto, CreateOperationUnsuitabilityItemsDto, UpdateOperationUnsuitabilityItemsDto, ListOperationUnsuitabilityItemsParameterDto>
    {
    }
}
