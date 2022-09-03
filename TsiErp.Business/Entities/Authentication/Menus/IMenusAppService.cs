using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Authentication.Dtos.Menus;
using Tsi.Core.Utilities.Results;

namespace TsiErp.Business.Entities.Authentication.Menus
{
    public interface IMenusAppService
    {
        Task<IDataResult<IList<ListMenusDto>>> GetListAsync();
    }
}
