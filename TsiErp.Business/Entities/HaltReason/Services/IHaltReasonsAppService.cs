using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;

namespace TsiErp.Business.Entities.HaltReason.Services
{
    public interface IHaltReasonsAppService : ICrudAppService<SelectHaltReasonsDto, ListHaltReasonsDto, CreateHaltReasonsDto, UpdateHaltReasonsDto, ListHaltReasonsParameterDto>
    {
    }
}
