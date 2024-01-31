using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services
{
    public interface IUserPermissionsAppService : ICrudAppService<SelectUserPermissionsDto, ListUserPermissionsDto, CreateUserPermissionsDto, UpdateUserPermissionsDto, ListUserPermissionsParameterDto>
    {
        Task<IDataResult<IList<SelectUserPermissionsDto>>> GetListAsyncByUserId(Guid userId);

        Task AllPermissionsAddedUser(Guid userId);
    }
}
