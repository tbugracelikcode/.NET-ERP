using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Employee;

namespace TsiErp.Business.Entities.Employee.BusinessRules
{
    public class EmployeeManager
    {
        public async Task CodeControl(IEmployeesRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IEmployeesRepository _repository, string code, Guid id, Employees entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IEmployeesRepository _repository, Guid id)
        {
        }
    }
}
