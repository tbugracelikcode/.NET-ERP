using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Shift;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Shift
{
    public interface IShiftsRepository : IEfCoreRepository<Shifts>
    {
    }
}
