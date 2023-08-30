using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;

namespace TsiErp.Business.Entities.OperationalSPC.Services
{
    public interface IOperationalSPCsAppService : ICrudAppService<SelectOperationalSPCsDto, ListOperationalSPCsDto, CreateOperationalSPCsDto, UpdateOperationalSPCsDto, ListOperationalSPCsParameterDto>
    {
    }
}
