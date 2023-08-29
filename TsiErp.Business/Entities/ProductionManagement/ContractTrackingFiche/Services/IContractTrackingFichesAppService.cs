using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;

namespace TsiErp.Business.Entities.ContractTrackingFiche.Services
{
    public interface IContractTrackingFichesAppService : ICrudAppService<SelectContractTrackingFichesDto, ListContractTrackingFichesDto, CreateContractTrackingFichesDto, UpdateContractTrackingFichesDto, ListContractTrackingFichesParameterDto>
    {
    }
}
