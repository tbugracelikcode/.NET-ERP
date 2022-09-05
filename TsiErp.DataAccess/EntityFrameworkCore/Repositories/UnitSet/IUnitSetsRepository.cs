using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.UnitSet;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnitSet
{
    public interface IUnitSetsRepository : IEfCoreRepository<UnitSets>
    {
    }
}
