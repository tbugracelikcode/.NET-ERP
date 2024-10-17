
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.CostManagement.CPR.Dtos;

namespace TsiErp.Business.Entities.CostManagement.CPR.Services
{
    public interface ICPRsAppService : ICrudAppService<SelectCPRsDto, ListCPRsDto, CreateCPRsDto, UpdateCPRsDto, ListCPRsParameterDto>
    {
        Task<IResult> DeleteMaterialCostAsync(Guid id);
        Task<IResult> DeleteManufacturingCostAsync(Guid id);
        Task<IResult> DeleteSetupCostAsync(Guid id);
    }
}
