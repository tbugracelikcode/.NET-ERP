using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;

namespace TsiErp.Business.Entities.UserGroup.Services
{
    public interface IUserGroupsAppService : ICrudAppService<SelectUserGroupsDto, ListUserGroupsDto, CreateUserGroupsDto, UpdateUserGroupsDto, ListUserGroupsParameterDto>
    {
    }
}
