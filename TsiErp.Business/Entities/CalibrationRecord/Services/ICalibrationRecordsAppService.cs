using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    public interface ICalibrationRecordsAppService : ICrudAppService<CalibrationRecords, SelectCalibrationRecordsDto, ListCalibrationRecordsDto, CreateCalibrationRecordsDto, UpdateCalibrationRecordsDto>
    {
    }
}
