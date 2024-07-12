using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Services
{
    public interface IProductionManagementParametersAppService : ICrudAppService<SelectProductionManagementParametersDto, ListProductionManagementParametersDto, CreateProductionManagementParametersDto, UpdateProductionManagementParametersDto, ListProductionManagementParametersParameterDto>
    {
        Task<IDataResult<SelectProductionManagementParametersDto>> GetProductionManagementParametersAsync();
    }
}