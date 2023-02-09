using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber;
using TsiErp.Entities.Entities.ProductReferanceNumber;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber
{
    [ServiceRegistration(typeof(IProductReferanceNumbersRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductReferanceNumbersRepository : EfCoreRepository<ProductReferanceNumbers>, IProductReferanceNumbersRepository
    {
        public EFCoreProductReferanceNumbersRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
