using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Entities.Menus;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Menus
{
    public interface IMenusRepository : IEfCoreRepository<TsiMenus>
    {
    }
}
