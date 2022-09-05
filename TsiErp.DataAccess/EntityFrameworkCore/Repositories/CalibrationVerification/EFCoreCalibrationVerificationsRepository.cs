using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification;
using TsiErp.Entities.Entities.CalibrationVerification;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification
{
    [ServiceRegistration(typeof(ICalibrationVerificationsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreCalibrationVerificationsRepository : EfCoreRepository<CalibrationVerifications>, ICalibrationVerificationsRepository
    {
        public EFCoreCalibrationVerificationsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
