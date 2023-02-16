using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.ProductsOperation;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperation
{
    [ServiceRegistration(typeof(IProductsOperationsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductsOperationsRepository : EfCoreRepository<ProductsOperations>, IProductsOperationsRepository
    {
        public EFCoreProductsOperationsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
