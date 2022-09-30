using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Operation;
using TsiErp.Entities.Entities.Operation.Dtos;

namespace TsiErp.Business.Entities.Operation.Services
{
    public interface IOperationsAppService : ICrudAppService<SelectOperationsDto, ListOperationsDto, CreateOperationsDto, UpdateOperationsDto, ListOperationsParameterDto>
    {
    }
}
