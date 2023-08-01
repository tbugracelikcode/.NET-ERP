using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.GeneralParameter.Services
{
    public interface IGeneralParametersAppService : ICrudAppService<SelectGeneralParametersDto, ListGeneralParametersDto, CreateGeneralParametersDto, UpdateGeneralParametersDto, ListGeneralParametersParameterDto>
    {
    }
}