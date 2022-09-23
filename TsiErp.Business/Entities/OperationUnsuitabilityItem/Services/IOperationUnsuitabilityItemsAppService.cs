using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Services
{
    public interface IOperationUnsuitabilityItemsAppService : ICrudAppService<SelectOperationUnsuitabilityItemsDto, ListOperationUnsuitabilityItemsDto, CreateOperationUnsuitabilityItemsDto, UpdateOperationUnsuitabilityItemsDto, ListOperationUnsuitabilityItemsParameterDto>
    {
    }
}
