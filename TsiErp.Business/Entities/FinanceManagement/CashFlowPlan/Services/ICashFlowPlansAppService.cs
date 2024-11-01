using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;

namespace TsiErp.Business.Entities.FinanceManagement.CashFlowPlan.Services
{
    public interface ICashFlowPlansAppService : ICrudAppService<SelectCashFlowPlansDto, ListCashFlowPlansDto, CreateCashFlowPlansDto, UpdateCashFlowPlansDto, ListCashFlowPlansParameterDto>
    {
    }
}
