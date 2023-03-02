using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Localizations.Resources.PurchaseUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.BusinessRules
{
    public class PurchaseUnsuitabilityReportManager
    {
        public async Task CodeControl(IPurchaseUnsuitabilityReportsRepository _repository, string ficheno, IStringLocalizer<PurchaseUnsuitabilityReportsResource> L)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheno))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IPurchaseUnsuitabilityReportsRepository _repository, string ficheno, Guid id, PurchaseUnsuitabilityReports entity, IStringLocalizer<PurchaseUnsuitabilityReportsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheno) && entity.FicheNo != ficheno)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IPurchaseUnsuitabilityReportsRepository _repository, Guid id)
        {
          
        }
    }
}
