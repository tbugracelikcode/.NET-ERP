using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Localizations.Resources.CalibrationVerifications.Page;

namespace TsiErp.Business.Entities.CalibrationVerification.BusinessRules
{
    public class CalibrationVerificationManager
    {
        public async Task CodeControl(ICalibrationVerificationsRepository _repository, string code, IStringLocalizer<CalibrationVerificationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ICalibrationVerificationsRepository _repository, string code, Guid id, CalibrationVerifications entity, IStringLocalizer<CalibrationVerificationsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ICalibrationVerificationsRepository _repository, Guid id)
        {
            
        }
    }
}
