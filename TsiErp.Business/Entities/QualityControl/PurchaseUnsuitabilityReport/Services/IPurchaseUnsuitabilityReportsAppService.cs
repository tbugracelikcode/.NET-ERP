using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    public interface IPurchaseUnsuitabilityReportsAppService : ICrudAppService<SelectPurchaseUnsuitabilityReportsDto, ListPurchaseUnsuitabilityReportsDto, CreatePurchaseUnsuitabilityReportsDto, UpdatePurchaseUnsuitabilityReportsDto, ListPurchaseUnsuitabilityReportsParameterDto>
    {
        Task<IDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>> GetListbyStartEndDateAsync(DateTime startDate, DateTime endDate);
    }
}
