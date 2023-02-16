using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.TemplateOperation;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperation
{
    [ServiceRegistration(typeof(ITemplateOperationsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreTemplateOperationsRepository : EfCoreRepository<TemplateOperations>, ITemplateOperationsRepository
    {
        public EFCoreTemplateOperationsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
