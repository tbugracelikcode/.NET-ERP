using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Menus;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.RolePermissions
{
    [ServiceRegistration(typeof(IRolePermissionsRepository), DependencyInjectionType.Singleton)]
    public class EfCoreRolePermissionsRepository : EfCoreRepository<TsiRolePermissions>, IRolePermissionsRepository
    {
        public EfCoreRolePermissionsRepository(TsiErpDbContext context) : base(context)
        {
        }
    }
}
