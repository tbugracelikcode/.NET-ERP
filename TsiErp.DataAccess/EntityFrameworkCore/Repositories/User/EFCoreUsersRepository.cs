using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.User;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.User
{
    [ServiceRegistration(typeof(IUsersRepository), DependencyInjectionType.Scoped)]
    public class EFCoreUsersRepository : EfCoreRepository<Users>, IUsersRepository
    {
        public EFCoreUsersRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
