using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Services
{
    public interface IFinalControlUnsuitabilityReportsAppService : ICrudAppService<SelectFinalControlUnsuitabilityReportsDto, ListFinalControlUnsuitabilityReportsDto, CreateFinalControlUnsuitabilityReportsDto, UpdateFinalControlUnsuitabilityReportsDto, ListFinalControlUnsuitabilityReportsParameterDto>
    {
    }
}
