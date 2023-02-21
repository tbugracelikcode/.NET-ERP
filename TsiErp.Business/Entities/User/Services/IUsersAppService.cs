using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.User.Dtos;

namespace TsiErp.Business.Entities.User.Services
{
    public interface IUsersAppService : ICrudAppService<SelectUsersDto, ListUsersDto, CreateUsersDto, UpdateUsersDto, ListUsersParameterDto>
    {
    }
}
