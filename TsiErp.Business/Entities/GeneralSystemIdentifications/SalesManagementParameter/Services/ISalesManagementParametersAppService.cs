using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.SalesManagementParameter.Services
{
    public interface ISalesManagementParametersAppService : ICrudAppService<SelectSalesManagementParametersDto, ListSalesManagementParametersDto, CreateSalesManagementParametersDto, UpdateSalesManagementParametersDto, ListSalesManagementParametersParameterDto>
    {
    }
}