using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.SalesPrice.Services
{
    public interface ISalesPricesAppService : ICrudAppService<SelectSalesPricesDto, ListSalesPricesDto, CreateSalesPricesDto, UpdateSalesPricesDto, ListSalesPricesParameterDto>
    {

    }
}
