using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.EquipmentRecord;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Localizations.Resources.EquipmentRecords.Page;

namespace TsiErp.Business.Entities.EquipmentRecord.BusinessRules
{
    public class EquipmentRecordManager
    {
        public async Task CodeControl(IEquipmentRecordsRepository _repository, string code, IStringLocalizer<EquipmentRecordsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IEquipmentRecordsRepository _repository, string code, Guid id, EquipmentRecords entity, IStringLocalizer<EquipmentRecordsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IEquipmentRecordsRepository _repository, Guid id, IStringLocalizer<EquipmentRecordsResource> L)
        {
            if (await _repository.AnyAsync(t => t.CalibrationRecords.Any(x => x.EquipmentID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }

            if (await _repository.AnyAsync(t => t.CalibrationVerifications.Any(x => x.EquipmentID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
