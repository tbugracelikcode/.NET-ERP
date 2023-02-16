using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    public interface IPaymentPlansAppService : ICrudAppService<SelectPaymentPlansDto, ListPaymentPlansDto, CreatePaymentPlansDto, UpdatePaymentPlansDto, ListPaymentPlansParameterDto>
    {
    }
}
