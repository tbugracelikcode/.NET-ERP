using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.SalesPropositionLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPropositionLine
{
    public interface ISalesPropositionLinesRepository : IEfCoreRepository<SalesPropositionLines>
    {
    }
}
