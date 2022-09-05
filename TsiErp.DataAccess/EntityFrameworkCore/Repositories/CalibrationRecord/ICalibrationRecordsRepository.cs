using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.Period;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord
{
    public interface ICalibrationRecordsRepository : IEfCoreRepository<CalibrationRecords>
    {
    }
}
