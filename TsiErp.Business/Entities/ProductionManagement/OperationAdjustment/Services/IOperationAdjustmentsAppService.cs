using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment.Dtos;

namespace TsiErp.Business.Entities.ProductionManagement.OperationAdjustment.Services
{
    public interface IOperationAdjustmentsAppService : ICrudAppService<SelectOperationAdjustmentsDto, ListOperationAdjustmentsDto, CreateOperationAdjustmentsDto, UpdateOperationAdjustmentsDto, ListOperationAdjustmentsParameterDto>
    {

        Task<int> GetTotalAdjustmentTimeAsync(Guid workOrderId);
    }
}
