using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Department;

namespace TsiErp.Business.Entities.Department.BusinessRules
{
    public class DepartmentManager
    {
        public async Task CodeControl(IDepartmentsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IDepartmentsRepository _repository, string code, Guid id, Departments entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IDepartmentsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.Employees.Any(x => x.DepartmentID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
            if (await _repository.AnyAsync(t => t.EquipmentRecords.Any(x => x.Department == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
