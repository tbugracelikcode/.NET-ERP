
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos;

namespace TsiErp.Business.Entities.CostManagement.CostPeriod.Services
{
    public interface ICostPeriodsAppService : ICrudAppService<SelectCostPeriodsDto, ListCostPeriodsDto, CreateCostPeriodsDto, UpdateCostPeriodsDto, ListCostPeriodsParameterDto>
    {
    }
}
