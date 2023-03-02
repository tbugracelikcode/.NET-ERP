using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Localizations.Resources.Shifts.Page;

namespace TsiErp.Business.Entities.Shift.BusinessRules
{
    public class ShiftManager
    {
        public async Task CodeControl(IShiftsRepository _repository, string code, int order, IStringLocalizer<ShiftsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            else if (await _repository.AnyAsync(t=>t.ShiftOrder == order))
            {
                throw new DuplicateCodeException(L["OrderNoControlManager"]);
            }
        }

        public async Task UpdateControl(IShiftsRepository _repository, string code, Guid id, Shifts entity, int order,IStringLocalizer<ShiftsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            else if ((await _repository.AnyAsync(t => t.Id != id && t.ShiftOrder == order) && entity.ShiftOrder != order))
            {
                throw new DuplicateCodeException(L["OrderNoControlManager"]);
            }
        }

        public async Task DeleteControl(IShiftsRepository _repository, Guid id)
        {

        }
    }
}
