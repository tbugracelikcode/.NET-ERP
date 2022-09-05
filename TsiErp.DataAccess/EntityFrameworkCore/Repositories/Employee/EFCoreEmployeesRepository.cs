using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee;
using TsiErp.Entities.Entities.Employee;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee
{
    [ServiceRegistration(typeof(IEmployeesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreEmployeesRepository : EfCoreRepository<Employees>, IEmployeesRepository
    {
        public EFCoreEmployeesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
