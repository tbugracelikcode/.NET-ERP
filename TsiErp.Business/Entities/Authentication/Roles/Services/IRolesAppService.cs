using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Authentication.Entities.Roles;

namespace TsiErp.Business.Entities.Authentication.Roles.Services
{
    public interface IRolesAppService : ICrudAppService<TsiRoles, SelectRolesDto, ListRolesDto, CreateRolesDto, UpdateRolesDto>
    {

    }
}
