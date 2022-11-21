using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.Menu;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Menu
{
    [ServiceRegistration(typeof(IMenusRepository), DependencyInjectionType.Scoped)]
    public class EFCoreMenusRepository : EfCoreRepository<Menus>, IMenusRepository
    {
        public EFCoreMenusRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
