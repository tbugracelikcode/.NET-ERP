using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;

namespace TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Services
{
    public interface IShipmentPlanningsAppService : ICrudAppService<SelectShipmentPlanningsDto, ListShipmentPlanningsDto, CreateShipmentPlanningsDto, UpdateShipmentPlanningsDto, ListShipmentPlanningsParameterDto>
    {
        Task<IDataResult<SelectShipmentPlanningLinesDto>> GetLinebyProductionOrderAsync(Guid productionOrderID);

        Task<IDataResult<SelectShipmentPlanningsDto>> GetbyProductionOrderAsync(Guid productionOrderID);

        Task<IDataResult<IList<ListShipmentPlanningsDto>>> ODGetListbyDateAsync(DateTime date);

        Task<IDataResult<SelectShipmentPlanningsDto>> ODGetbyDateAsync(DateTime selectedDate);
    }
}
