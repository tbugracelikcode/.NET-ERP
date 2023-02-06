using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ByDateStockMovement;
using TsiErp.Entities.Entities.ByDateStockMovement;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ByDateStockMovement
{
    [ServiceRegistration(typeof(IByDateStockMovementsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreByDateStockMovementsRepository : EfCoreRepository<ByDateStockMovements>, IByDateStockMovementsRepository
    {
        public EFCoreByDateStockMovementsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
