using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Currency;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Currency;

namespace TsiErp.Business.Entities.Currency.BusinessRules
{
    public class CurrencyManager
    {
        public async Task CodeControl(ICurrenciesRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ICurrenciesRepository _repository, string code, Guid id, Currencies entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ICurrenciesRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.CurrentAccountCards.Any(x => x.CurrencyID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }

            if (await _repository.AnyAsync(t => t.ExchangeRates.Any(x => x.CurrencyID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }

            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.CurrencyID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
