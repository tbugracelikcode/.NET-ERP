using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.Shift;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift
{
    [ServiceRegistration(typeof(IShiftsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreShiftsRepository : EfCoreRepository<Shifts>, IShiftsRepository
    {
        public EFCoreShiftsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
