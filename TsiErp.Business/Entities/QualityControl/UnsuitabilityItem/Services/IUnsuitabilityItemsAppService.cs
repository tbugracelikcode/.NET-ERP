using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityItem.Services
{
    public interface IUnsuitabilityItemsAppService : ICrudAppService<SelectUnsuitabilityItemsDto, ListUnsuitabilityItemsDto, CreateUnsuitabilityItemsDto, UpdateUnsuitabilityItemsDto, ListUnsuitabilityItemsParameterDto>
    {
    }
}
