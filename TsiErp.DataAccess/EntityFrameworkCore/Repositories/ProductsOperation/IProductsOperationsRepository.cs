using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductsOperation;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperation
{
    public interface IProductsOperationsRepository : IEfCoreRepository<ProductsOperations>
    {
    }
}
