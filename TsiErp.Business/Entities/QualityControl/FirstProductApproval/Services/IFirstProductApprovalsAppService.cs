using System.ComponentModel;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;

namespace TsiErp.Business.Entities.FirstProductApproval.Services
{
    public interface IFirstProductApprovalsAppService : ICrudAppService<SelectFirstProductApprovalsDto, ListFirstProductApprovalsDto, CreateFirstProductApprovalsDto, UpdateFirstProductApprovalsDto, ListFirstProductApprovalsParameterDto>
    {
    }
}
