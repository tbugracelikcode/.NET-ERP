using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Currency.Dtos;
using Tsi.Core.Services.BusinessCoreServices;

namespace TsiErp.Business.Entities.Currency.Services
{
    public interface ICurrenciesAppService : ICrudAppService<SelectCurrenciesDto, ListCurrenciesDto, CreateCurrenciesDto, UpdateCurrenciesDto, ListCurrenciesParameterDto>

    {
    }
}
