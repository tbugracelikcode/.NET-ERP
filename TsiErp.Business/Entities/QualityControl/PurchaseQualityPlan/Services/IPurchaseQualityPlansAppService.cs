using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;

namespace TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Services
{
    public interface IPurchaseQualityPlansAppService : ICrudAppService<SelectPurchaseQualityPlansDto, ListPurchaseQualityPlansDto, CreatePurchaseQualityPlansDto, UpdatePurchaseQualityPlansDto, ListPurchaseQualityPlansParameterDto>
    {
        Task<IResult> DeleteLineAsync(Guid id);

        Task<IDataResult<SelectPurchaseQualityPlansDto>> GetbyCurrentAccountandProductAsync(Guid CurrentAccountCardID, Guid ProductID);

        Task<int> RevisionNoControlAsync(Guid purchaseQualityControlPlanId, string revisionNo);
    }
}
