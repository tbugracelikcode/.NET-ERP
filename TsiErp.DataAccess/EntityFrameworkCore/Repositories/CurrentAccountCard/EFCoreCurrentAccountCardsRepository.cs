using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Entities.Entities.CurrentAccountCard;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CurrentAccountCard
{
    [ServiceRegistration(typeof(ICurrentAccountCardsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreCurrentAccountCardsRepository : EfCoreRepository<CurrentAccountCards>, ICurrentAccountCardsRepository
    {
        public EFCoreCurrentAccountCardsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
