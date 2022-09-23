using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Product.Dtos;
using Tsi.Application.Contract.Services.EntityFrameworkCore;

namespace TsiErp.Business.Entities.Product.Services
{
    public interface IProductsAppService : ICrudAppService<SelectProductsDto, ListProductsDto, CreateProductsDto, UpdateProductsDto, ListProductsParameterDto>
    {
    }
}
