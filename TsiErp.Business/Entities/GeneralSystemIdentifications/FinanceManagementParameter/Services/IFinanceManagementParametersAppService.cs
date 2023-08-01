using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Services
{
    public interface IFinanceManagementParametersAppService: ICrudAppService<SelectFinanceManagementParametersDto, ListFinanceManagementParametersDto, CreateFinanceManagementParametersDto, UpdateFinanceManagementParametersDto, ListFinanceManagementParametersParameterDto>
    {
        Task<IDataResult<SelectFinanceManagementParametersDto>> GetFinanceManagementParametersAsync();
    }
}
