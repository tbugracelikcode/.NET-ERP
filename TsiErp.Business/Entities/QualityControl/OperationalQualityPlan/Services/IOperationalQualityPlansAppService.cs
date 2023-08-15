using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;

namespace TsiErp.Business.Entities.OperationalQualityPlan.Services
{
    public interface IOperationalQualityPlansAppService : ICrudAppService<SelectOperationalQualityPlansDto, ListOperationalQualityPlansDto, CreateOperationalQualityPlansDto, UpdateOperationalQualityPlansDto, ListOperationalQualityPlansParameterDto>
    {

    }
}
