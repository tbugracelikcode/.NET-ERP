using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.ContractTrackingFiche.Services
{
    public interface IContractTrackingFichesAppService : ICrudAppService<SelectContractTrackingFichesDto, ListContractTrackingFichesDto, CreateContractTrackingFichesDto, UpdateContractTrackingFichesDto, ListContractTrackingFichesParameterDto>
    {
        Task<IResult> DeleteLineAsync(Guid id);
        Task<IResult> DeleteAmountEntryLine(Guid id);

        Task<IDataResult<IList<SelectContractTrackingFicheLinesDto>>> GetLineListbyWorkOrderIDAsync(Guid workOrderID);
    }
}
