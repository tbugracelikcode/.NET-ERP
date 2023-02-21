using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    public interface IPaymentPlansAppService : ICrudAppService<SelectPaymentPlansDto, ListPaymentPlansDto, CreatePaymentPlansDto, UpdatePaymentPlansDto, ListPaymentPlansParameterDto>
    {
    }
}
