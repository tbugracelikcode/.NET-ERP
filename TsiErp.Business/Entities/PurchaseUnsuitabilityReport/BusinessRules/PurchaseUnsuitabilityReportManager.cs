using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.BusinessRules
{
    public class PurchaseUnsuitabilityReportManager
    {
        public async Task CodeControl(IPurchaseUnsuitabilityReportsRepository _repository, string ficheno)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheno))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IPurchaseUnsuitabilityReportsRepository _repository, string ficheno, Guid id, PurchaseUnsuitabilityReports entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheno) && entity.FicheNo != ficheno)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IPurchaseUnsuitabilityReportsRepository _repository, Guid id)
        {
          
        }
    }
}
