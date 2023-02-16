using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;

namespace TsiErp.Business.Entities.ProductsOperation.Services
{
    public interface IProductsOperationsAppService : ICrudAppService<SelectProductsOperationsDto, ListProductsOperationsDto, CreateProductsOperationsDto, UpdateProductsOperationsDto, ListProductsOperationsParameterDto>
    {
    }
}
