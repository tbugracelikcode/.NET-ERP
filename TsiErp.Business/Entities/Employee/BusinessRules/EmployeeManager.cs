using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Localizations.Resources.Employees.Page;

namespace TsiErp.Business.Entities.Employee.BusinessRules
{
    public class EmployeeManager
    {
        public async Task CodeControl(IEmployeesRepository _repository, string code, IStringLocalizer<EmployeesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IEmployeesRepository _repository, string code, Guid id, Employees entity, IStringLocalizer<EmployeesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IEmployeesRepository _repository, Guid id)
        {
        }
    }
}
