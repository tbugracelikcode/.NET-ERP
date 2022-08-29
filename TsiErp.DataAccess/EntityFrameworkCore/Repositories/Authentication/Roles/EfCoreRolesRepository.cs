using Microsoft.EntityFrameworkCore;
using Tsi.Authentication.Entities.Roles;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Roles
{
    [ServiceRegistration(typeof(IRolesRepository), DependencyInjectionType.Singleton)]
    public class EfCoreRolesRepository : EfCoreRepository<TsiRoles>, IRolesRepository
    {
        public EfCoreRolesRepository(TsiErpDbContext context) : base(context)
        {
        }
    }
}
