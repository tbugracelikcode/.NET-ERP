using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Services
{
    public interface IPeriodsAppService : ICrudAppService<SelectPeriodsDto, ListPeriodsDto, CreatePeriodsDto, UpdatePeriodsDto, ListPeriodsParameterDto>
    {
    }
}
