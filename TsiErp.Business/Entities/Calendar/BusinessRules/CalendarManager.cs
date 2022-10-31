using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Calendar;
using TsiErp.Entities.Entities.Calendar;

namespace TsiErp.Business.Entities.Calendar.BusinessRules
{
    public class CalendarManager
    {
        public async Task CodeControl(ICalendarsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodda bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(ICalendarsRepository _repository, string code, Guid id, Calendars entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodda bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(ICalendarsRepository _repository, Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.CalendarLines);
            
        }
    }
}
