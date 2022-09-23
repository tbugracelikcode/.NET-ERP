using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ShippingAdress;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShippingAdress
{
    [ServiceRegistration(typeof(IShippingAdressesRepository), DependencyInjectionType.Scoped)]
    public class EFCoreShippingAdressesRepository : EfCoreRepository<ShippingAdresses>, IShippingAdressesRepository
    {
        public EFCoreShippingAdressesRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
