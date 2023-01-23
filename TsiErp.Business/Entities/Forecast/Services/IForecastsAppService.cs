using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.Forecast.Services
{
    public interface IForecastsAppService : ICrudAppService<SelectForecastsDto, ListForecastsDto, CreateForecastsDto, UpdateForecastsDto, ListForecastsParameterDto>
    {

    }
}
