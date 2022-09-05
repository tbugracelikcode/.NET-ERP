using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.UnitSet;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnitSet
{
    [ServiceRegistration(typeof(IUnitSetsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreUnitSetsRepository : EfCoreRepository<UnitSets>, IUnitSetsRepository
    {
        public EFCoreUnitSetsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
