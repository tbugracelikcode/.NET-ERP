using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationRecord;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord
{
    [ServiceRegistration(typeof(ICalibrationRecordsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreCalibrationRecordsRepository : EfCoreRepository<CalibrationRecords>, ICalibrationRecordsRepository
    {
        public EFCoreCalibrationRecordsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
