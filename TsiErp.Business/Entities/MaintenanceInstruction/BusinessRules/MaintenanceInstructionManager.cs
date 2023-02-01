using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.MaintenanceInstruction;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.MaintenanceInstruction.BusinessRules
{
    public class MaintenanceInstructionManager
    {
        public async Task CodeControl(IMaintenanceInstructionsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IMaintenanceInstructionsRepository _repository, string code, Guid id, MaintenanceInstructions entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
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
