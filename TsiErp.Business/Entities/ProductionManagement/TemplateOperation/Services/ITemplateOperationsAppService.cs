using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    public interface ITemplateOperationsAppService : ICrudAppService<SelectTemplateOperationsDto, ListTemplateOperationsDto, CreateTemplateOperationsDto, UpdateTemplateOperationsDto, ListTemplateOperationsParameterDto>
    {
    }
}
