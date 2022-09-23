using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.SalesProposition.Dtos;

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    public interface ISalesPropositionsAppService : ICrudAppService<SelectSalesPropositionsDto, ListSalesPropositionsDto, CreateSalesPropositionsDto, UpdateSalesPropositionsDto, ListSalesPropositionsParamaterDto>
    {
    }
}
