using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.SalesProposition;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition
{
    [ServiceRegistration(typeof(ISalesPropositionsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreSalesPropositionsRepository : EfCoreRepository<SalesPropositions>, ISalesPropositionsRepository
    {
        public EFCoreSalesPropositionsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
