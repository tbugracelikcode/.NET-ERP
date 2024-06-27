using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;

namespace TsiErp.Business.Entities.Route.Services
{
    public interface IRoutesAppService : ICrudAppService<SelectRoutesDto, ListRoutesDto, CreateRoutesDto, UpdateRoutesDto, ListRoutesParameterDto>
    {
         Task<IDataResult<List<ListProductsOperationsDto>>> GetProductsOperationAsync(Guid productId);

        Task<IDataResult<SelectRouteLinesDto>> GetLinebyProductsOperationIDAsync(Guid productsOperationID);
    }
}
