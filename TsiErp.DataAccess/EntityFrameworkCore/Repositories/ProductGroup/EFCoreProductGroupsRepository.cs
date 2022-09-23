using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductGroup;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductGroup
{
    [ServiceRegistration(typeof(IProductGroupsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreProductGroupsRepository : EfCoreRepository<ProductGroups>, IProductGroupsRepository
    {
        public EFCoreProductGroupsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
