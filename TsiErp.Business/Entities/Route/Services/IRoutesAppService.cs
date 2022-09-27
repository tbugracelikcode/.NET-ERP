using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.Route.Dtos;

namespace TsiErp.Business.Entities.Route.Services
{
    public interface IRoutesAppService : ICrudAppService<SelectRoutesDto, ListRoutesDto, CreateRoutesDto, UpdateRoutesDto, ListRoutesParameterDto>
    {
    }
}
