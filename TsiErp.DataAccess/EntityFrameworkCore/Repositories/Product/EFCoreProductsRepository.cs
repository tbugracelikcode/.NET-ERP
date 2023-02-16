using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.Product;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Product
{
    [ServiceRegistration(typeof(IProductsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductsRepository : EfCoreRepository<Products>, IProductsRepository
    {
        public EFCoreProductsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
