using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;

namespace TsiErp.Business.Entities.Menu.Services
{
    public interface IMenusAppService : ICrudAppService<SelectMenusDto, ListMenusDto, CreateMenusDto, UpdateMenusDto, ListMenusParameterDto>
    {

        Task<IDataResult<IList<SelectMenusDto>>> GetListbyParentIDAsync(Guid parentID);
    }
}
