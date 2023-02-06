using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.GrandTotalStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.GrandTotalStockMovement
{
    [ServiceRegistration(typeof(IGrandTotalStockMovementsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreGrandTotalStockMovementsRepository : EfCoreRepository<GrandTotalStockMovements>, IGrandTotalStockMovementsRepository
    {
        public EFCoreGrandTotalStockMovementsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
