using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;

namespace TsiErp.Business.Entities.QualityControl.ControlCondition.Services
{
    public interface IControlConditionsAppService : ICrudAppService<SelectControlConditionsDto, ListControlConditionsDto, CreateControlConditionsDto, UpdateControlConditionsDto, ListControlConditionsParameterDto>
    {
    }
}