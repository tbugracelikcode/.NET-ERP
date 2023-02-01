using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.BusinessRules
{
    public class FinalControlUnsuitabilityReportManager
    {
        public async Task CodeControl(IFinalControlUnsuitabilityReportsRepository _repository, string ficheno)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheno))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IFinalControlUnsuitabilityReportsRepository _repository, string ficheno, Guid id, FinalControlUnsuitabilityReports entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheno) && entity.FicheNo != ficheno)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IFinalControlUnsuitabilityReportsRepository _repository, Guid id)
        {

        }
    }
}
