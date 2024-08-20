using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;

namespace TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Services
{
    public interface IOperationalQualityPlansAppService : ICrudAppService<SelectOperationalQualityPlansDto, ListOperationalQualityPlansDto, CreateOperationalQualityPlansDto, UpdateOperationalQualityPlansDto, ListOperationalQualityPlansParameterDto>
    {
        Task<IResult> DeleteLineAsync(Guid id);
        Task<IResult> DeleteOperationPictureAsync(Guid id);
        Task<IDataResult<SelectOperationalQualityPlansDto>> GetbyOperationProductAsync(Guid operationID, Guid productID);

        Task<int> RevisionNoControlAsync(Guid operationQualityControlPlanId, string revisionNo);
    }
}
