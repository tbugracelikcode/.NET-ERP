using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;

namespace TsiErp.Business.Entities.BankAccount.Services
{
    public interface IBankAccountsAppService : ICrudAppService<SelectBankAccountsDto, ListBankAccountsDto, CreateBankAccountsDto, UpdateBankAccountsDto, ListBankAccountsParameterDto>
    {
    }
}
