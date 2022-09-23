using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.CustomerComplaintItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.CustomerComplaintItem
{
    public interface ICustomerComplaintItemsRepository : IEfCoreRepository<CustomerComplaintItems>
    {
    }
}
