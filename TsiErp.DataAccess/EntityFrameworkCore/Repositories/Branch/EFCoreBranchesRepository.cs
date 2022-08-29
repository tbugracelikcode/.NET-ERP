using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Branch;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch
{
    [ServiceRegistration(typeof(IBranchesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreBranchesRepository : EfCoreRepository<TsiErpDbContext,Branches>, IBranchesRepository
    {
    }
}
