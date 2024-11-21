using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlanLine.Dtos;

namespace TsiErp.Business.Entities.FinanceManagement.CashFlowPlan.Services
{
    public interface ICashFlowPlansAppService : ICrudAppService<SelectCashFlowPlansDto, ListCashFlowPlansDto, CreateCashFlowPlansDto, UpdateCashFlowPlansDto, ListCashFlowPlansParameterDto>
    {
        Task<IDataResult<SelectCashFlowPlanLinesDto>> GetLineAsync(Guid id);

        Task<IDataResult<SelectCashFlowPlanLinesDto>> CreateUpdateLineAsync(SelectCashFlowPlanLinesDto input);

        Task<IDataResult<IList<SelectCashFlowPlanLinesDto>>> GetLineListAsync(DateTime date, Guid currenctId, Guid bankAccountId);
    }
}
