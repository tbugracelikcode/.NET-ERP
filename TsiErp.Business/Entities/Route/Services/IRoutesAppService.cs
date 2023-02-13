using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.Route.Dtos;

namespace TsiErp.Business.Entities.Route.Services
{
    public interface IRoutesAppService : ICrudAppService<SelectRoutesDto, ListRoutesDto, CreateRoutesDto, UpdateRoutesDto, ListRoutesParameterDto>
    {
         Task<IDataResult<List<ListProductsOperationsDto>>> GetProductsOperationAsync(Guid productId);

        Task<IDataResult<SelectRoutesDto>> GetSelectListAsync(Guid productId);


        //Task<IDataResult<List<SelectProductsOperationsDto>>> GetProductsOperationLinesAsync(Guid productId);
    }
}
