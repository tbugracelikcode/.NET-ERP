using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.CompanyCheck.Dtos;

namespace TsiErp.Business.Entities.CompanyCheck.Services
{
    public interface ICompanyChecksAppService : ICrudAppService<SelectCompanyChecksDto, ListCompanyChecksDto, CreateCompanyChecksDto, UpdateCompanyChecksDto, ListCompanyChecksParameterDto>
    {
    }
}
