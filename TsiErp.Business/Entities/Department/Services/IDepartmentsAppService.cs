using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.Department;
using TsiErp.Entities.Entities.Department.Dtos;

namespace TsiErp.Business.Entities.Department.Services
{
    public interface IDepartmentsAppService : ICrudAppService<Departments, SelectDepartmentsDto, ListDepartmentsDto, CreateDepartmentsDto, UpdateDepartmentsDto>
    {
    }
}
