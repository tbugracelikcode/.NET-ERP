using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.SalesPropositionLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPropositionLine
{
    [ServiceRegistration(typeof(ISalesPropositionLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreSalesPropositionLinesRepository : EfCoreRepository<SalesPropositionLines>, ISalesPropositionLinesRepository
    {
        public EFCoreSalesPropositionLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
