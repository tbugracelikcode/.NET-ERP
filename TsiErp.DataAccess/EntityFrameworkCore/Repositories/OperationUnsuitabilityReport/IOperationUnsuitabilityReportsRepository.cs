using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityReport
{
    public interface IOperationUnsuitabilityReportsRepository : IEfCoreRepository<OperationUnsuitabilityReports>
    {
    }
}
