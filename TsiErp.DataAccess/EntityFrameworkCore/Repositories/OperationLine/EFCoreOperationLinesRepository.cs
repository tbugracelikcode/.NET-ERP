using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.OperationLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationLine
{
    [ServiceRegistration(typeof(IOperationLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreOperationLinesRepository : EfCoreRepository<OperationLines>, IOperationLinesRepository
    {
        public EFCoreOperationLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
