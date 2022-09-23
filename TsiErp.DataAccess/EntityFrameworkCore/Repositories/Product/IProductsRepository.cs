using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.Product;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Product
{
    public interface IProductsRepository : IEfCoreRepository<Products>
    {
    }
}
