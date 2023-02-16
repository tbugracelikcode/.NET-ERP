using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.BillsofMaterial;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterial
{
    [ServiceRegistration(typeof(IBillsofMaterialsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreBillsofMaterialsRepository : EfCoreRepository<BillsofMaterials>, IBillsofMaterialsRepository
    {
        public EFCoreBillsofMaterialsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
