using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Currency.Dtos;

namespace TsiErp.Business.Entities.Currency.Services
{
    public interface ICurrenciesAppService : ICrudAppService<SelectCurrenciesDto, ListCurrenciesDto, CreateCurrenciesDto, UpdateCurrenciesDto, ListCurrenciesParameterDto>

    {
    }
}
