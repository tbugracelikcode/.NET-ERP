using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;

namespace TsiErp.Business.Entities.LeanProduction.GeneralOEE.Services
{
    public interface IGeneralOEEsAppService : ICrudAppService<SelectGeneralOEEsDto, ListGeneralOEEsDto, CreateGeneralOEEsDto, UpdateGeneralOEEsDto, ListGeneralOEEsParameterDto>
    {
        Task<IDataResult<SelectGeneralOEEsDto>> GetbyDateAsync(DateTime date);
    }
}
