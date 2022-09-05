using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;

namespace TsiErp.Business.Entities.CalibrationVerification.Services
{
    public interface ICalibrationVerificationsAppService : ICrudAppService<CalibrationVerifications, SelectCalibrationVerificationsDto, ListCalibrationVerificationsDto, CreateCalibrationVerificationsDto, UpdateCalibrationVerificationsDto,ListCalibrationVerificationsParameterDto>
    {
    }
}
