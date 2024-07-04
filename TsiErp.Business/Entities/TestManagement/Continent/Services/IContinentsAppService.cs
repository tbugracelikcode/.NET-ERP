using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.TestManagement.Continent.Dtos;

namespace TsiErp.Business.Entities.Continent.Services
{
    public interface IContinentsAppService : ICrudAppService<SelectContinentsDto, ListContinentsDto, CreateContinentsDto, UpdateContinentsDto, ListContinentsParameterDto>
    {
    }
}
