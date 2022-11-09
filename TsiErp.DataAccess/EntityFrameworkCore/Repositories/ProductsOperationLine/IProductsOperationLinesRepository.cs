using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductsOperationLine;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperationLine
{
    public interface IProductsOperationLinesRepository : IEfCoreRepository<ProductsOperationLines>
    {
    }
}
