using System;
using System.Collections.Generic;
using System.Text;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using Tsi.Logging.EntityFrameworkCore.Entities;

namespace Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore
{
    public class EfCoreLogsRepository : EfCoreRepository<LogDbContext, Logs>, ILogsRepository
    {

    }
}
