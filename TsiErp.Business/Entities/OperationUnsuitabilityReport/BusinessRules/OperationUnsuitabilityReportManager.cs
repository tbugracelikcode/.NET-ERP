using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.BusinessRules
{
    public class OperationUnsuitabilityReportManager
    {
        public async Task CodeControl(IOperationUnsuitabilityReportsRepository _repository, string ficheno)
        {
            if (await _repository.AnyAsync(t => t.FicheNo == ficheno))
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IOperationUnsuitabilityReportsRepository _repository, string ficheno, Guid id, OperationUnsuitabilityReports entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.FicheNo == ficheno) && entity.FicheNo != ficheno)
            {
                throw new DuplicateCodeException("Aynı numaralı bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IOperationUnsuitabilityReportsRepository _repository, Guid id)
        {

        }
    }
}
