using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification.Dtos;

namespace TsiErp.Business.Entities.CalibrationVerification.Services
{
    public interface ICalibrationVerificationsAppService : ICrudAppService<SelectCalibrationVerificationsDto, ListCalibrationVerificationsDto, CreateCalibrationVerificationsDto, UpdateCalibrationVerificationsDto,ListCalibrationVerificationsParameterDto>
    {
    }
}
