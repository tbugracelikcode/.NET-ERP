using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Enums;

namespace TsiErp.Business.Entities.Shift.BusinessRules
{
    public class ShiftManager
    {
        public async Task CodeControl(IShiftsRepository _repository, string code, int order)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }

            else if (await _repository.AnyAsync(t=>t.ShiftOrder == order))
            {
                throw new DuplicateCodeException("Aynı vardiya sıra numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IShiftsRepository _repository, string code, Guid id, Shifts entity, int order)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }

            else if ((await _repository.AnyAsync(t => t.Id != id && t.ShiftOrder == order) && entity.ShiftOrder != order))
            {
                throw new DuplicateCodeException("Aynı vardiya sıra numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IShiftsRepository _repository, Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.ShiftLines);

        }
    }
}
