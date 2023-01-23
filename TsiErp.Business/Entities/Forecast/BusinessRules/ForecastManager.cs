using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Forecast;
using TsiErp.Entities.Entities.Forecast;

namespace TsiErp.Business.Entities.Forecast.BusinessRules
{
    public class ForecastManager
    {
        public async Task CodeControl(IForecastsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IForecastsRepository _repository, string code, Guid id, Forecasts entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IForecastsRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.ForecastLines);

                var line = entity.ForecastLines.Where(t => t.Id == lineId).FirstOrDefault();

                
            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

            }
        }
    }
}
