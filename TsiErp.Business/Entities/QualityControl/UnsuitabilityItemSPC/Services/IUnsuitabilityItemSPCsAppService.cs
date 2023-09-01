using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;

namespace TsiErp.Business.Entities.UnsuitabilityItemSPC.Services
{
    public interface IUnsuitabilityItemSPCsAppService : ICrudAppService<SelectUnsuitabilityItemSPCsDto, ListUnsuitabilityItemSPCsDto, CreateUnsuitabilityItemSPCsDto, UpdateUnsuitabilityItemSPCsDto, ListUnsuitabilityItemSPCsParameterDto>
    {
    }
}
