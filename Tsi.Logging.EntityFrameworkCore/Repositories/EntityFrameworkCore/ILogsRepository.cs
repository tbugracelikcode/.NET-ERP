using System;
using System.Collections.Generic;
using System.Text;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.Logging.EntityFrameworkCore.Entities;

namespace Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore
{
    public interface ILogsRepository : IEfCoreRepository<Logs>
    {
    }
}
