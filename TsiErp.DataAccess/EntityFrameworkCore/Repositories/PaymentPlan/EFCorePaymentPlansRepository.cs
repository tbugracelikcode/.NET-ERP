using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.PaymentPlan;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan
{
    [ServiceRegistration(typeof(IPaymentPlansRepository), DependencyInjectionType.Scoped)]
    public class EFCorePaymentPlansRepository : EfCoreRepository<PaymentPlans>, IPaymentPlansRepository
    {
        public EFCorePaymentPlansRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
