using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    public interface IWorkOrdersAppService : ICrudAppService<SelectWorkOrdersDto, ListWorkOrdersDto, CreateWorkOrdersDto, UpdateWorkOrdersDto, ListWorkOrdersParameterDto>
    {
        Task<IDataResult<SelectWorkOrdersDto>> GetbyLinkedWorkOrderAsync(Guid linkedWorkOrderID);

        Task<IDataResult<IList<SelectWorkOrdersDto>>> GetSelectListbyProductionOrderAsync(Guid productionOrderID);

        Task<IDataResult<SelectWorkOrdersDto>> UpdateChangeStationAsync(UpdateWorkOrdersDto input);

        Task<IDataResult<SelectWorkOrdersDto>> UpdateWorkOrderSplitAsync(UpdateWorkOrdersDto input);

        Task<IDataResult<SelectWorkOrdersDto>> GetbyProductionOrderOperationRouteAsync(Guid productionOrderID, Guid productsOperationID, Guid routeID);
    }
}
