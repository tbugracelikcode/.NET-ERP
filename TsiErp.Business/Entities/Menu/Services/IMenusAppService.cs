using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.Menu.Dtos;

namespace TsiErp.Business.Entities.Menu.Services
{
    public interface IMenusAppService : ICrudAppService<SelectMenusDto, ListMenusDto, CreateMenusDto, UpdateMenusDto, ListMenusParameterDto>
    {
    }
}
