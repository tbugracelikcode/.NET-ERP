using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Services
{
    public interface IUnsuitabilityTypesItemsAppService : ICrudAppService<SelectUnsuitabilityTypesItemsDto, ListUnsuitabilityTypesItemsDto, CreateUnsuitabilityTypesItemsDto, UpdateUnsuitabilityTypesItemsDto, ListUnsuitabilityTypesItemsParameterDto>
    {
    }
}
