using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.User;
using TsiErp.Entities.Entities.User.Dtos;

namespace TsiErp.Business.Entities.User.Services
{
    public interface IUsersAppService : ICrudAppService<SelectUsersDto, ListUsersDto, CreateUsersDto, UpdateUsersDto, ListUsersParameterDto>
    {
    }
}
