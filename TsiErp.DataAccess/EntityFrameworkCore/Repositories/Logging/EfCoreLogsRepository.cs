using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.EntityframeworkCore;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Logging;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Logging
{
    [ServiceRegistration(typeof(ILogsRepository), DependencyInjectionType.Scoped)]
    public class EfCoreLogsRepository : EfCoreRepository<Logs>, ILogsRepository
    {
        public EfCoreLogsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
