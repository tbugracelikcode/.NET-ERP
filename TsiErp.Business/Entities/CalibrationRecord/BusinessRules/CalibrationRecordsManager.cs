using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Localizations.Resources.CalibrationRecords.Page;

namespace TsiErp.Business.Entities.CalibrationRecord.BusinessRules
{
    public class CalibrationRecordsManager
    {
        public async Task CodeControl(ICalibrationRecordsRepository _repository, string code, IStringLocalizer<CalibrationRecordsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(ICalibrationRecordsRepository _repository, string code, Guid id, CalibrationRecords entity, IStringLocalizer<CalibrationRecordsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(ICalibrationRecordsRepository _repository, Guid id)
        {
            
        }
    }
}
