using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Services
{
    public interface IFinalControlUnsuitabilityItemsAppService : ICrudAppService<SelectFinalControlUnsuitabilityItemsDto, ListFinalControlUnsuitabilityItemsDto, CreateFinalControlUnsuitabilityItemsDto, UpdateFinalControlUnsuitabilityItemsDto, ListFinalControlUnsuitabilityItemsParameterDto>
    {
    }
}
