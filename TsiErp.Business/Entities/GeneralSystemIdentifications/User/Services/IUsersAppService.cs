using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;

namespace TsiErp.Business.Entities.User.Services
{
    public interface IUsersAppService : ICrudAppService<SelectUsersDto, ListUsersDto, CreateUsersDto, UpdateUsersDto, ListUsersParameterDto>
    {
        Task<IDataResult<SelectUsersDto>> GetAsyncByUserNameAndPassword(string userName, string password);
        Task<IDataResult<SelectUsersDto>> GetAsyncRegisterUser(string registerUserName, string registerPassword);
    }
}
