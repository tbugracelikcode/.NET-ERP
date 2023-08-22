using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;

namespace TsiErp.Business.Entities.QualityControl.ContractQualityPlan.Services
{
    public interface IContractQualityPlansAppService : ICrudAppService<SelectContractQualityPlansDto, ListContractQualityPlansDto, CreateContractQualityPlansDto, UpdateContractQualityPlansDto, ListContractQualityPlansParameterDto>
    {
        Task<IResult> DeleteLineAsync(Guid id);
        Task<IResult> DeleteContractPictureAsync(Guid id);
        Task<IResult> DeleteContractOperationAsync(Guid id);
    }
}
