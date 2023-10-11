using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.QualityControl.Report8D.Dtos;

namespace TsiErp.Business.Entities.Report8D.Services
{
    public interface IReport8DsAppService : ICrudAppService<SelectReport8DsDto, ListReport8DsDto, CreateReport8DsDto, UpdateReport8DsDto, ListReport8DsParameterDto>
    {
    }
}
