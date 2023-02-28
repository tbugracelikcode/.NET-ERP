using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesPriceLine.Dtos;

namespace TsiErp.Business.Entities.SalesPrice.Services
{
    public interface ISalesPricesAppService : ICrudAppService<SelectSalesPricesDto, ListSalesPricesDto, CreateSalesPricesDto, UpdateSalesPricesDto, ListSalesPricesParameterDto>
    {
        Task<IDataResult<IList<SelectSalesPriceLinesDto>>> GetSelectLineListAsync(Guid productId);
    }
}
