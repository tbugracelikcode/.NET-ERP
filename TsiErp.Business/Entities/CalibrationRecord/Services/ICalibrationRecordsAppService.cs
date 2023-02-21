using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    public interface ICalibrationRecordsAppService : ICrudAppService< SelectCalibrationRecordsDto, ListCalibrationRecordsDto, CreateCalibrationRecordsDto, UpdateCalibrationRecordsDto,ListCalibrationRecordsParameterDto>
    {
    }
}
