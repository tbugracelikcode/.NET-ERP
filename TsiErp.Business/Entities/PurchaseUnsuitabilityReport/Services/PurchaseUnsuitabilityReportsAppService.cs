using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Localizations.Resources.PurchaseUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IPurchaseUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseUnsuitabilityReportsAppService : ApplicationService<PurchaseUnsuitabilityReportsResource>, IPurchaseUnsuitabilityReportsAppService
    {
        public PurchaseUnsuitabilityReportsAppService(IStringLocalizer<PurchaseUnsuitabilityReportsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> CreateAsync(CreatePurchaseUnsuitabilityReportsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>> GetListAsync(ListPurchaseUnsuitabilityReportsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateAsync(UpdatePurchaseUnsuitabilityReportsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
