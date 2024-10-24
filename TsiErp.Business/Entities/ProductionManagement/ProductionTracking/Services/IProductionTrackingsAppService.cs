using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    public interface IProductionTrackingsAppService : ICrudAppService<SelectProductionTrackingsDto, ListProductionTrackingsDto, CreateProductionTrackingsDto, UpdateProductionTrackingsDto, ListProductionTrackingsParameterDto>
    {
        Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListbyWorkOrderIDAsync(Guid workOrderID);

        Task<IDataResult<IList<SelectProductionTrackingsDto>>> GetListbyOprStartDateRangeIsEqualAsync(DateTime startDate,DateTime endDate);
        Task<IDataResult<IList<ListProductionTrackingsDto>>> GetListDashboardProductGroupAsync(DateTime startDate, DateTime endDate);
    }
}
