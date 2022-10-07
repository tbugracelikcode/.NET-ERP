using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.EquipmentRecord;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.EquipmentRecord;

namespace TsiErp.Business.Entities.EquipmentRecord.BusinessRules
{
    public class EquipmentRecordManager
    {
        public async Task CodeControl(IEquipmentRecordsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IEquipmentRecordsRepository _repository, string code, Guid id, EquipmentRecords entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IEquipmentRecordsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.CalibrationRecords.Any(x => x.EquipmentID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }

            if (await _repository.AnyAsync(t => t.CalibrationVerifications.Any(x => x.EquipmentID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
