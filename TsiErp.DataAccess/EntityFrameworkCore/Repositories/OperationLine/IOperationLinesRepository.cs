using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.OperationLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationLine
{
    public interface IOperationLinesRepository : IEfCoreRepository<OperationLines>
    {
    }
}
