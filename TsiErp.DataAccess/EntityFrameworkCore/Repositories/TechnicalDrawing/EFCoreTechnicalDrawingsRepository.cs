using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing;
using TsiErp.Entities.Entities.TechnicalDrawing;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing
{
    [ServiceRegistration(typeof(ITechnicalDrawingsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreTechnicalDrawingsRepository : EfCoreRepository<TechnicalDrawings>, ITechnicalDrawingsRepository
    {
        public EFCoreTechnicalDrawingsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
