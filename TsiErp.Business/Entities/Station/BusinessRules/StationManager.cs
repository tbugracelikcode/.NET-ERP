using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Station;

namespace TsiErp.Business.Entities.Station.BusinessRules
{
    public class StationManager
    {
        public async Task CodeControl(IStationsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IStationsRepository _repository, string code, Guid id, Stations entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IStationsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.OperationLines.Any(x => x.StationID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
