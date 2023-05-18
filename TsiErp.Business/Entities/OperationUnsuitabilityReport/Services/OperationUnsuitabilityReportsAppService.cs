using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport.Dtos;
using TsiErp.Localizations.Resources.OperationUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityReportsAppService : ApplicationService<OperationUnsuitabilityReportsResource>, IOperationUnsuitabilityReportsAppService
    {
        public OperationUnsuitabilityReportsAppService(IStringLocalizer<OperationUnsuitabilityReportsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> CreateAsync(CreateOperationUnsuitabilityReportsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListOperationUnsuitabilityReportsDto>>> GetListAsync(ListOperationUnsuitabilityReportsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateAsync(UpdateOperationUnsuitabilityReportsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
