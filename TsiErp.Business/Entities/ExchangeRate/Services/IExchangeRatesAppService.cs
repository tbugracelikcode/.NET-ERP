using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.ExchangeRate;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;

namespace TsiErp.Business.Entities.ExchangeRate.Services
{
    public interface IExchangeRatesAppService : ICrudAppService<SelectExchangeRatesDto, ListExchangeRatesDto, CreateExchangeRatesDto, UpdateExchangeRatesDto, ListExchangeRatesParameterDto>
    {
    }
}
