using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    public interface IWorkOrdersAppService : ICrudAppService<SelectWorkOrdersDto, ListWorkOrdersDto, CreateWorkOrdersDto, UpdateWorkOrdersDto, ListWorkOrdersParameterDto>
    {
        Task<IDataResult<SelectWorkOrdersDto>> GetbyProductionOrderIdAsync(Guid? id);
    }
}
