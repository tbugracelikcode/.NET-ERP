using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
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
