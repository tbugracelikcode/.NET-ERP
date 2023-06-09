using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;

namespace TsiErp.Business.Entities.ExchangeRate.Services
{
    public interface IExchangeRatesAppService : ICrudAppService<SelectExchangeRatesDto, ListExchangeRatesDto, CreateExchangeRatesDto, UpdateExchangeRatesDto, ListExchangeRatesParameterDto>
    {
    }
}
