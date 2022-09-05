using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Employee;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.EquipmentRecord;
using TsiErp.Entities.Entities.EquipmentRecord;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.EquipmentRecord
{
    [ServiceRegistration(typeof(IEquipmentRecordsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreEquipmentRecordsRepository : EfCoreRepository<EquipmentRecords>, IEquipmentRecordsRepository
    {
        public EFCoreEquipmentRecordsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
