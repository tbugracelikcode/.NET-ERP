using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Warehouse;
using TsiErp.Entities.Entities.WareHouse;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Warehouse
{
    [ServiceRegistration(typeof(IWarehousesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreWarehousesRepository : EfCoreRepository<Warehouses>, IWarehousesRepository
    {
        public EFCoreWarehousesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
