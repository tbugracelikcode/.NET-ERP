using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShippingAdress;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.ShippingAdress;

namespace TsiErp.Business.Entities.ShippingAdress.BusinessRules
{
    public class ShippingAdressesManager
    {
        public async Task CodeControl(IShippingAdressesRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IShippingAdressesRepository _repository, string code, Guid id, ShippingAdresses entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IShippingAdressesRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.ShippingAdressID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
