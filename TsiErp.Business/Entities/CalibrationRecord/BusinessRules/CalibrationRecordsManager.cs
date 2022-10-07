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

namespace TsiErp.Business.Entities.CalibrationRecord.BusinessRules
{
    public class CalibrationRecordsManager
    {
        public async Task CodeControl(ICalibrationRecordsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ICalibrationRecordsRepository _repository, string code, Guid id, CalibrationRecords entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ICalibrationRecordsRepository _repository, Guid id)
        {
            
        }
    }
}
