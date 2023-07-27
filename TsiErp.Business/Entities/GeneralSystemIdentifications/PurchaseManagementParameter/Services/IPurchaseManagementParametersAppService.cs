using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Services
{
    public interface IPurchaseManagementParametersAppService : ICrudAppService<SelectPurchaseManagementParametersDto, ListPurchaseManagementParametersDto, CreatePurchaseManagementParametersDto, UpdatePurchaseManagementParametersDto, ListPurchaseManagementParametersParameterDto>
    {
    }
}