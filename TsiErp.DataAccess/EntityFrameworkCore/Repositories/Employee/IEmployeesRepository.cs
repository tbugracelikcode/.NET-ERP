using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Employee;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee
{
    public interface IEmployeesRepository : IEfCoreRepository<Employees>
    {
    }
}
