using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;

namespace TsiErp.Business.Entities.UserGroup.Services
{
    public interface IUserGroupsAppService : ICrudAppService<SelectUserGroupsDto, ListUserGroupsDto, CreateUserGroupsDto, UpdateUserGroupsDto, ListUserGroupsParameterDto>
    {
    }
}
