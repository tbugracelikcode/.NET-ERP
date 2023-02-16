using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Forecast;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Forecast
{
    public interface IForecastsRepository : IEfCoreRepository<Forecasts>
    {
    }
}
