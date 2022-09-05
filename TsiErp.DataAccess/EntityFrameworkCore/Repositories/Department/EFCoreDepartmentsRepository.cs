using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department;
using TsiErp.Entities.Entities.Department;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department
{
    [ServiceRegistration(typeof(IDepartmentsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreDepartmentsRepository : EfCoreRepository<Departments>, IDepartmentsRepository
    {
        public EFCoreDepartmentsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
