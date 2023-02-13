using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.EntityFrameworkCore.Respositories.UnitOfWork;
using TsiErp.Entities.Entities.Branch;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch
{
    public interface IBranchesRepository : IEfCoreRepository<Branches>
    {
    }
}
