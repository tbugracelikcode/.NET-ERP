using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Localizations.Resources.MaintenanceInstructions.Page;

namespace TsiErp.Business.Entities.MaintenanceInstruction.BusinessRules
{
    public class MaintenanceInstructionManager
    {
        public async Task CodeControl(IMaintenanceInstructionsRepository _repository, string code, IStringLocalizer<MaintenanceInstructionsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IMaintenanceInstructionsRepository _repository, string code, Guid id, MaintenanceInstructions entity, IStringLocalizer<MaintenanceInstructionsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IMaintenanceInstructionsRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.MaintenanceInstructionLines);

                var line = entity.MaintenanceInstructionLines.Where(t => t.Id == lineId).FirstOrDefault();
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);
            }
        }
    }
}
