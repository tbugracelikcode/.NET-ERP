using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem
{
    [ServiceRegistration(typeof(ICustomerComplaintItemsRepository), DependencyInjectionType.Scoped)]
    public class EFCoreCustomerComplaintItemsRepository : EfCoreRepository<CustomerComplaintItems>, ICustomerComplaintItemsRepository
    {
        public EFCoreCustomerComplaintItemsRepository(TsiErpDbContext dbContext) : base(dbContext)
        {
        }
    }
}
