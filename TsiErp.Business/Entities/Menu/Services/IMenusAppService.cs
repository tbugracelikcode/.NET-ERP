using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Menu.Dtos;

namespace TsiErp.Business.Entities.Menu.Services
{
    public interface IMenusAppService : ICrudAppService<SelectMenusDto, ListMenusDto, CreateMenusDto, UpdateMenusDto, ListMenusParameterDto>
    {
    }
}
