using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeItem.Dtos;
using TsiErp.Localizations.Resources.ProductionOrderChangeItems.Page;

namespace TsiErp.Business.Entities.ProductionOrderChangeItem.Services
{
    [ServiceRegistration(typeof(IProductionOrderChangeItemsAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrderChangeItemsAppService : ApplicationService<ProductionOrderChangeItemsResource>, IProductionOrderChangeItemsAppService
    {
        public ProductionOrderChangeItemsAppService(IStringLocalizer<ProductionOrderChangeItemsResource> l) : base(l)
        {
        }

        public Task<IDataResult<SelectProductionOrderChangeItemsDto>> CreateAsync(CreateProductionOrderChangeItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectProductionOrderChangeItemsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListProductionOrderChangeItemsDto>>> GetListAsync(ListProductionOrderChangeItemsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectProductionOrderChangeItemsDto>> UpdateAsync(UpdateProductionOrderChangeItemsDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectProductionOrderChangeItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
