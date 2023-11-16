using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;

namespace TsiErp.Business.Entities.ContractTrackingFiche.Services
{
    public interface IContractTrackingFichesAppService : ICrudAppService<SelectContractTrackingFichesDto, ListContractTrackingFichesDto, CreateContractTrackingFichesDto, UpdateContractTrackingFichesDto, ListContractTrackingFichesParameterDto>
    {
        Task<IResult> DeleteLineAsync(Guid id);
        Task<IResult> DeleteAmountEntryLine(Guid id);
    }
}
