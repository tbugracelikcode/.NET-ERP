using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Localizations.Resources.FinalControlUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.BusinessRules
{
    public class FinalControlUnsuitabilityReportManager
    {
        public async Task CodeControl(IFinalControlUnsuitabilityReportsRepository _repository, string ficheno, IStringLocalizer<FinalControlUnsuitabilityReportsResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheno))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IFinalControlUnsuitabilityReportsRepository _repository, string ficheno, Guid id, FinalControlUnsuitabilityReports entity, IStringLocalizer<FinalControlUnsuitabilityReportsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheno) && entity.FicheNo != ficheno)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IFinalControlUnsuitabilityReportsRepository _repository, Guid id)
        {

        }
    }
}
