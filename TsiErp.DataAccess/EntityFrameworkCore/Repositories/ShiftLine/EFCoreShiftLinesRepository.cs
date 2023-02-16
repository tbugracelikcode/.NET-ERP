using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.ShiftLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShiftLine
{
    [ServiceRegistration(typeof(IShiftLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreShiftLinesRepository : EfCoreRepository<ShiftLines>, IShiftLinesRepository
    {
        public EFCoreShiftLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
