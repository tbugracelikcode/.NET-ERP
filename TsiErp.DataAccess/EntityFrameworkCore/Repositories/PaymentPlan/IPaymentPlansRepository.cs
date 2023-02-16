using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.PaymentPlan;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan
{
    public interface IPaymentPlansRepository : IEfCoreRepository<PaymentPlans>
    {
    }
}
