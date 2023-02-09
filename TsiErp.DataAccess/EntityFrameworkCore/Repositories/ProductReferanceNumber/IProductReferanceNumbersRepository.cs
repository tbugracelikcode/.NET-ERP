using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductReferanceNumber;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductReferanceNumber
{
    public interface IProductReferanceNumbersRepository : IEfCoreRepository<ProductReferanceNumbers>
    {
    }
}
