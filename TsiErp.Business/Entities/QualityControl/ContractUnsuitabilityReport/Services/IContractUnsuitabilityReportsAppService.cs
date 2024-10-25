using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.ContractUnsuitabilityReport.Services
{
    public interface IContractUnsuitabilityReportsAppService : ICrudAppService<SelectContractUnsuitabilityReportsDto, ListContractUnsuitabilityReportsDto, CreateContractUnsuitabilityReportsDto, UpdateContractUnsuitabilityReportsDto, ListContractUnsuitabilityReportsParameterDto>
    {
        Task<IDataResult<IList<ListContractUnsuitabilityReportsDto>>> GetListbyStartEndDateAsync(DateTime startDate, DateTime endDate);
    }
}
