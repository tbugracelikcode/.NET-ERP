using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using Tsi.Logging.EntityFrameworkCore.Entities;

namespace Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore
{
    [ServiceRegistration(typeof(ILogsRepository), DependencyInjectionType.Scoped)]
    public class EfCoreLogsRepository : EfCoreRepository<Logs>, ILogsRepository
    {
        public EfCoreLogsRepository(LogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
