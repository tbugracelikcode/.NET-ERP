using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Localizations.Resources.OperationUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.BusinessRules
{
    public class OperationUnsuitabilityReportManager
    {
        public async Task CodeControl(IOperationUnsuitabilityReportsRepository _repository, string ficheno, IStringLocalizer<OperationUnsuitabilityReportsResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheno))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IOperationUnsuitabilityReportsRepository _repository, string ficheno, Guid id, OperationUnsuitabilityReports entity, IStringLocalizer<OperationUnsuitabilityReportsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheno) && entity.FicheNo != ficheno)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IOperationUnsuitabilityReportsRepository _repository, Guid id)
        {

        }
    }
}
