using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    public interface IContractProductionTrackingsAppService : ICrudAppService<SelectContractProductionTrackingsDto, ListContractProductionTrackingsDto, CreateContractProductionTrackingsDto, UpdateContractProductionTrackingsDto, ListContractProductionTrackingsParameterDto>
    {
        Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId);

    }
}
