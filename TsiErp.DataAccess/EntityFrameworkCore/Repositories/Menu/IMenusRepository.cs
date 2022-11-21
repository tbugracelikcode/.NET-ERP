using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Menu;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Menu
{
    public interface IMenusRepository : IEfCoreRepository<Menus>
    {
    }
}
