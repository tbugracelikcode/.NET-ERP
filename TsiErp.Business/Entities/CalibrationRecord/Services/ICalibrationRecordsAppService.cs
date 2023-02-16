using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    public interface ICalibrationRecordsAppService : ICrudAppService< SelectCalibrationRecordsDto, ListCalibrationRecordsDto, CreateCalibrationRecordsDto, UpdateCalibrationRecordsDto,ListCalibrationRecordsParameterDto>
    {
    }
}
