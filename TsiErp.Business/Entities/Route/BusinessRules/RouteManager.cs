using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route;
using TsiErp.Entities.Entities.Route;

namespace TsiErp.Business.Entities.Route.BusinessRules
{
    public class RouteManager
    {
        public async Task CodeControl(IRoutesRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IRoutesRepository _repository, string code, Guid id, Routes entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IRoutesRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.RouteLines);

                var line = entity.RouteLines.Where(t => t.Id == lineId).FirstOrDefault();

            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

            }
        }
    }
}
