using System;
using System.Collections.Generic;
using System.Text;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Logging;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Logging
{
    public interface ILogsRepository : IEfCoreRepository<Logs>
    {
    }
}
