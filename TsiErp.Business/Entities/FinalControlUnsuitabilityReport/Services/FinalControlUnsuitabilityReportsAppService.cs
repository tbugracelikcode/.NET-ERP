using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport.Dtos;
using TsiErp.Localizations.Resources.FinalControlUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class FinalControlUnsuitabilityReportsAppService : ApplicationService<FinalControlUnsuitabilityReportsResource>, IFinalControlUnsuitabilityReportsAppService
    {
        public FinalControlUnsuitabilityReportsAppService(IStringLocalizer<FinalControlUnsuitabilityReportsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> CreateAsync(CreateFinalControlUnsuitabilityReportsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListFinalControlUnsuitabilityReportsDto>>> GetListAsync(ListFinalControlUnsuitabilityReportsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> UpdateAsync(UpdateFinalControlUnsuitabilityReportsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
