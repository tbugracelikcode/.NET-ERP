using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Department;
using TsiErp.Entities.Entities.Department;
using TsiErp.Localizations.Resources.Departments.Page;

namespace TsiErp.Business.Entities.Department.BusinessRules
{
    public class DepartmentManager
    {
        public async Task CodeControl(IDepartmentsRepository _repository, string code, IStringLocalizer<DepartmentsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IDepartmentsRepository _repository, string code, Guid id, Departments entity, IStringLocalizer<DepartmentsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IDepartmentsRepository _repository, Guid id, IStringLocalizer<DepartmentsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Employees.Any(x => x.DepartmentID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            if (await _repository.AnyAsync(t => t.EquipmentRecords.Any(x => x.Department == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
