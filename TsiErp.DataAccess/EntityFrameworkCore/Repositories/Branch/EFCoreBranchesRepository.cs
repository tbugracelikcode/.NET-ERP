using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using Tsi.IoC.Tsi.DependencyResolvers;
using TsiErp.Entities.Entities.Branch;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch
{
    [ServiceRegistration(typeof(IBranchesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreBranchesRepository : EfCoreRepository<TsiErpDbContext, Branches>, IBranchesRepository
    {

    }
}
