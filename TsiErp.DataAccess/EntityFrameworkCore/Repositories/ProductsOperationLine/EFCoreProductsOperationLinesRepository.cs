using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductsOperationLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperationLine
{
    [ServiceRegistration(typeof(IProductsOperationLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductsOperationLinesRepository : EfCoreRepository<ProductsOperationLines>, IProductsOperationLinesRepository
    {
        public EFCoreProductsOperationLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
