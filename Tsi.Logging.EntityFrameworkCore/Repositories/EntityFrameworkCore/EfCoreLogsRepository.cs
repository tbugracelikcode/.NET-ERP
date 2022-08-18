using System;
using System.Collections.Generic;
using System.Text;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using Tsi.IoC.Tsi.DependencyResolvers;
using Tsi.Logging.EntityFrameworkCore.Entities;

namespace Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore
{
    [ServiceRegistration(typeof(ILogsRepository), DependencyInjectionType.Scoped)]
    public class EfCoreLogsRepository : EfCoreRepository<LogDbContext, Logs>, ILogsRepository
    {

    }
}
