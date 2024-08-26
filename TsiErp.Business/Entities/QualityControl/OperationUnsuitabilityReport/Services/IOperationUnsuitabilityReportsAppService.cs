using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Services
{
    public interface IOperationUnsuitabilityReportsAppService : ICrudAppService<SelectOperationUnsuitabilityReportsDto, ListOperationUnsuitabilityReportsDto, CreateOperationUnsuitabilityReportsDto, UpdateOperationUnsuitabilityReportsDto, ListOperationUnsuitabilityReportsParameterDto>
    {
        Task<IDataResult<IList<SelectOperationUnsuitabilityReportsDto>>> GetListbyOprStartDateRangeIsEqualAsync(DateTime startDate, DateTime endDate);
    }
}
