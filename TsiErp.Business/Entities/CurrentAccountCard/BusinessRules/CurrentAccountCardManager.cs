using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CurrentAccountCard;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CurrentAccountCard;

namespace TsiErp.Business.Entities.CurrentAccountCard.BusinessRules
{
    public class CurrentAccountCardManager
    {
        public async Task CodeControl(ICurrentAccountCardsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ICurrentAccountCardsRepository _repository, string code, Guid id, CurrentAccountCards entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ICurrentAccountCardsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.CurrentAccountCardID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
            if (await _repository.AnyAsync(t => t.ShippingAdresses.Any(x => x.CustomerCardID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
