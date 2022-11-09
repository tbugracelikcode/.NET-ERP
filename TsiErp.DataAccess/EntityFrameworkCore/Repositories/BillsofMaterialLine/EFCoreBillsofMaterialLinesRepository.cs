using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.BillsofMaterialLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterialLine
{
    [ServiceRegistration(typeof(IBillsofMaterialLinesRepository), DependencyInjectionType.Scoped)]

    public class EFCoreBillsofMaterialLinesRepository : EfCoreRepository<BillsofMaterialLines>, IBillsofMaterialLinesRepository
    {
        public EFCoreBillsofMaterialLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
