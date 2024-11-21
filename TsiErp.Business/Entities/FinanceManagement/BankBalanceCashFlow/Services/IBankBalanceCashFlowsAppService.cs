using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlow.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankBalanceCashFlowLine.Dtos;

namespace TsiErp.Business.Entities.FinanceManagement.BankBalanceCashFlow.Services
{
    public interface IBankBalanceCashFlowsAppService : ICrudAppService<SelectBankBalanceCashFlowsDto, ListBankBalanceCashFlowsDto, CreateBankBalanceCashFlowsDto, UpdateBankBalanceCashFlowsDto, ListBankBalanceCashFlowsParameterDto>
    {
        Task<IResult> DeleteLinesLineAsync(Guid id);

        Task<IDataResult<IList<SelectBankBalanceCashFlowLinesDto>>> GetLineListbyDateAsync(DateTime Date);
    }
}
