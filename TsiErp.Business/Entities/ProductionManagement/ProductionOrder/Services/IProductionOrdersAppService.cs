using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.ReportDtos;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    public interface IProductionOrdersAppService : ICrudAppService<SelectProductionOrdersDto, ListProductionOrdersDto, CreateProductionOrdersDto, UpdateProductionOrdersDto, ListProductionOrdersParameterDto>
    {
        Task<IDataResult<SelectProductionOrdersDto>> ConverttoProductionOrder(CreateProductionOrdersDto input);

        Task<IDataResult<IList<ListProductionOrdersDto>>> GetNotCanceledListAsync(ListProductionOrdersParameterDto input);

        Task<IDataResult<IList<ListProductionOrdersDto>>> GetCanceledListAsync(ListProductionOrdersParameterDto input);

        Task<IDataResult<IList<RawMaterialRequestFormReportDto>>> CreateRawMaterialRequestFormReportAsync(Guid productionOrderId);

        Task<IDataResult<SelectProductionOrdersDto>> UpdateOccuredAmountEntryAsync(UpdateProductionOrdersDto input);

        Task<IDataResult<SelectProductionOrdersDto>> UpdateChangeTechDrawingAsync(UpdateProductionOrdersDto input);

        Task<IDataResult<IList<SelectProductionOrdersDto>>> GetSelectListbyLinkedProductionOrder(Guid id);

    }
}
