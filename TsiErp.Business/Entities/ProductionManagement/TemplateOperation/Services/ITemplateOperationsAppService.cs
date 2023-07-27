using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    public interface ITemplateOperationsAppService : ICrudAppService<SelectTemplateOperationsDto, ListTemplateOperationsDto, CreateTemplateOperationsDto, UpdateTemplateOperationsDto, ListTemplateOperationsParameterDto>
    {
        Task<IDataResult<IList<SelectTemplateOperationUnsuitabilityItemsDto>>> GetUnsuitabilityItemsAsync(Guid workCenterId,Guid templateOperationId);
    }
}
