using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.WorkOrder.Dtos;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    public interface IWorkOrdersAppService : ICrudAppService<SelectWorkOrdersDto, ListWorkOrdersDto, CreateWorkOrdersDto, UpdateWorkOrdersDto, ListWorkOrdersParameterDto>
    {
    }
}
