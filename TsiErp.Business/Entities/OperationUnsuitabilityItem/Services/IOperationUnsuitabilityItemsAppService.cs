using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.OperationUnsuitabilityItem.Services
{
    public interface IOperationUnsuitabilityItemsAppService : ICrudAppService<SelectOperationUnsuitabilityItemsDto, ListOperationUnsuitabilityItemsDto, CreateOperationUnsuitabilityItemsDto, UpdateOperationUnsuitabilityItemsDto, ListOperationUnsuitabilityItemsParameterDto>
    {
    }
}
