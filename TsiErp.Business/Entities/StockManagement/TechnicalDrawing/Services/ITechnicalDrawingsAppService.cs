using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;

namespace TsiErp.Business.Entities.TechnicalDrawing.Services
{
    public interface ITechnicalDrawingsAppService : ICrudAppService<SelectTechnicalDrawingsDto, ListTechnicalDrawingsDto, CreateTechnicalDrawingsDto, UpdateTechnicalDrawingsDto, ListTechnicalDrawingsParameterDto>
    {
        Task<IDataResult<IList<SelectTechnicalDrawingsDto>>> GetSelectListAsync(Guid productId);
    }
}
