using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services
{
    public interface IStockManagementParametersAppService : ICrudAppService<SelectStockManagementParametersDto, ListStockManagementParametersDto, CreateStockManagementParametersDto, UpdateStockManagementParametersDto, ListStockManagementParametersParameterDto>
    {
        Task<IDataResult<SelectStockManagementParametersDto>> GetStockManagementParametersAsync();
    }
}