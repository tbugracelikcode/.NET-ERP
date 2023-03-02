using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Localizations.Resources.CustomerComplaintItems.Page;

namespace TsiErp.Business.Entities.CustomerComplaintItem.BusinessRules
{
    public class CustomerComplaintItemManager
    {
        public async Task CodeControl(ICustomerComplaintItemsRepository _repository, string code, IStringLocalizer<CustomerComplaintItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ICustomerComplaintItemsRepository _repository, string code, Guid id, CustomerComplaintItems entity, IStringLocalizer<CustomerComplaintItemsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ICustomerComplaintItemsRepository _repository, Guid id)
        {
        }
    }
}
