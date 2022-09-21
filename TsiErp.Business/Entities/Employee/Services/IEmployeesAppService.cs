using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Department;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.Employee.Dtos;

namespace TsiErp.Business.Entities.Employee.Services
{
    public interface IEmployeesAppService : ICrudAppService<SelectEmployeesDto, ListEmployeesDto, CreateEmployeesDto, UpdateEmployeesDto, ListEmployeesParameterDto>
    {
    }
}
