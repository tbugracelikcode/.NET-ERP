using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using Tsi.Core.Services.BusinessCoreServices;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    public interface IProductGroupsAppService : ICrudAppService<SelectProductGroupsDto, ListProductGroupsDto, CreateProductGroupsDto, UpdateProductGroupsDto, ListProductGroupsParameterDto>
    {
    }
}
