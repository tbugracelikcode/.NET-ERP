using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.WorkOrder.Dtos;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    public interface IWorkOrdersAppService : ICrudAppService<SelectWorkOrdersDto, ListWorkOrdersDto, CreateWorkOrdersDto, UpdateWorkOrdersDto, ListWorkOrdersParameterDto>
    {
    }
}
