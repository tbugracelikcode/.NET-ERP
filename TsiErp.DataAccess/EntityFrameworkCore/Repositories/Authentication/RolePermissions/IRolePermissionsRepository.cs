using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.RolePermissions
{
    public interface IRolePermissionsRepository : IEfCoreRepository<TsiRolePermissions>
    {
    }
}
