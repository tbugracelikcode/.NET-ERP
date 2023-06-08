using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    public interface IPurchaseUnsuitabilityReportsAppService : ICrudAppService<SelectPurchaseUnsuitabilityReportsDto, ListPurchaseUnsuitabilityReportsDto, CreatePurchaseUnsuitabilityReportsDto, UpdatePurchaseUnsuitabilityReportsDto, ListPurchaseUnsuitabilityReportsParameterDto>
    {
    }
}
