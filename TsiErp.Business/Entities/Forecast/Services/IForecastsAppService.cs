using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;

namespace TsiErp.Business.Entities.Forecast.Services
{
    public interface IForecastsAppService : ICrudAppService<SelectForecastsDto, ListForecastsDto, CreateForecastsDto, UpdateForecastsDto, ListForecastsParameterDto>
    {

    }
}
