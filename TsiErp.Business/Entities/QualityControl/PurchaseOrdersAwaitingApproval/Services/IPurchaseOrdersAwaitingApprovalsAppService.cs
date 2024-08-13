using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrdersAwaitingApproval.Services
{
    public interface IPurchaseOrdersAwaitingApprovalsAppService : ICrudAppService<SelectPurchaseOrdersAwaitingApprovalsDto, ListPurchaseOrdersAwaitingApprovalsDto, CreatePurchaseOrdersAwaitingApprovalsDto, UpdatePurchaseOrdersAwaitingApprovalsDto, ListPurchaseOrdersAwaitingApprovalsParameterDto>
    {
        Task<IDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>> UpdateCancelQCApprovalAsync(UpdatePurchaseOrdersAwaitingApprovalsDto input);
    }
}
