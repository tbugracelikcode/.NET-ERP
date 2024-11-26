using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.EximBankReeskont.Dtos;

namespace TsiErp.Business.Entities.FinanceManagement.EximBankReeskont.Services
{
    public interface IEximBankReeskontsAppService : ICrudAppService<SelectEximBankReeskontsDto, ListEximBankReeskontsDto, CreateEximBankReeskontsDto, UpdateEximBankReeskontsDto, ListEximBankReeskontsParameterDto>
    {
        
    }
}
