using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Route;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route
{
    public interface IRoutesRepository : IEfCoreRepository<Routes>
    {
    }
}
