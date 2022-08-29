using Microsoft.EntityFrameworkCore;
using Tsi.Authentication.Entities.Menus;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Menus
{
    [ServiceRegistration(typeof(IMenusRepository), DependencyInjectionType.Singleton)]
    public class EfCoreMenusRepository : EfCoreRepository<TsiMenus>, IMenusRepository
    {
        public EfCoreMenusRepository(TsiErpDbContext context) : base(context)
        {
        }
    }
}
