using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.QualityControlParameter.Services
{
    public interface IQualityControlParametersAppService : ICrudAppService<SelectQualityControlParametersDto, ListQualityControlParametersDto, CreateQualityControlParametersDto, UpdateQualityControlParametersDto, ListQualityControlParametersParameterDto>
    {
    }
}