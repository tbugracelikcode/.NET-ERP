using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;

namespace TsiErp.Business.Entities.ProductionOrderChangeReport.Services
{
    public interface IProductionOrderChangeReportsAppService : ICrudAppService<SelectProductionOrderChangeReportsDto, ListProductionOrderChangeReportsDto, CreateProductionOrderChangeReportsDto, UpdateProductionOrderChangeReportsDto, ListProductionOrderChangeReportsParameterDto>
    {

    }
}
