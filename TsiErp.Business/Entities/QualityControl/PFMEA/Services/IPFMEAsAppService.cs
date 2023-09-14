using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos;

namespace TsiErp.Business.Entities.PFMEA.Services
{
    public interface IPFMEAsAppService : ICrudAppService<SelectPFMEAsDto, ListPFMEAsDto, CreatePFMEAsDto, UpdatePFMEAsDto, ListPFMEAsParameterDto>
    {
    }
}
