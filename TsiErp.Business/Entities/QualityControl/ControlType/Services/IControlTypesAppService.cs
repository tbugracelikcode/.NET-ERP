using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;

namespace TsiErp.Business.Entities.QualityControl.ControlType.Services
{
    public interface IControlTypesAppService : ICrudAppService<SelectControlTypesDto, ListControlTypesDto, CreateControlTypesDto, UpdateControlTypesDto, ListControlTypesParameterDto>
    {
    }
}
