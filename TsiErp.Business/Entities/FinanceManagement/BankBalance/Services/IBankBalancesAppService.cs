using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.BankBalance.Dtos;

namespace TsiErp.Business.Entities.FinanceManagement.BankBalance.Services
{
    public interface IBankBalancesAppService : ICrudAppService<SelectBankBalancesDto, ListBankBalancesDto, CreateBankBalancesDto, UpdateBankBalancesDto, ListBankBalancesParameterDto>
    {

        Task<IDataResult<IList<SelectBankBalancesDto>>> GetListbyDateAsync(DateTime Date, Guid BankId);
    }
}
