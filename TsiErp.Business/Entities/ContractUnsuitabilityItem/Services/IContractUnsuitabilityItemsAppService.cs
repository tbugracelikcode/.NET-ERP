using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Services
{
    public interface IContractUnsuitabilityItemsAppService : ICrudAppService<SelectContractUnsuitabilityItemsDto, ListContractUnsuitabilityItemsDto, CreateContractUnsuitabilityItemsDto, UpdateContractUnsuitabilityItemsDto, ListContractUnsuitabilityItemsParameterDto>
    {
    }
}
