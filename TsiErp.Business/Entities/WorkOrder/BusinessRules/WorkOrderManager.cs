using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.WorkOrder;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Localizations.Resources.WorkOrders.Page;

namespace TsiErp.Business.Entities.WorkOrder.BusinessRules
{
    public class WorkOrderManager
    {
        public async Task CodeControl(IWorkOrdersRepository _repository, string code, IStringLocalizer<WorkOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IWorkOrdersRepository _repository, string code, Guid id, WorkOrders entity, IStringLocalizer<WorkOrdersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IWorkOrdersRepository _repository, Guid id)
        {
            
        }
    }
}
