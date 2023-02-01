using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.UserGroup;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UserGroup
{
    [ServiceRegistration(typeof(IUserGroupsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreUserGroupsRepository : EfCoreRepository<UserGroups>, IUserGroupsRepository
    {
        public EFCoreUserGroupsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
