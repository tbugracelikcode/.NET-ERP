using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CalibrationVerification;

namespace TsiErp.Business.Entities.CalibrationVerification.BusinessRules
{
    public class CalibrationVerificationManager
    {
        public async Task CodeControl(ICalibrationVerificationsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ICalibrationVerificationsRepository _repository, string code, Guid id, CalibrationVerifications entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ICalibrationVerificationsRepository _repository, Guid id)
        {
            
        }
    }
}
