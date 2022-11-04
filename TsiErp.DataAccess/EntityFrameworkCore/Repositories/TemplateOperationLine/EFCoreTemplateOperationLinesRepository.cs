using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.TemplateOperationLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperationLine
{
    [ServiceRegistration(typeof(ITemplateOperationLinesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreTemplateOperationLinesRepository : EfCoreRepository<TemplateOperationLines>, ITemplateOperationLinesRepository
    {
        public EFCoreTemplateOperationLinesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
