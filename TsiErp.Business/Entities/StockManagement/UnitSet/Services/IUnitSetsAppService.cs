using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    public interface IUnitSetsAppService : ICrudAppService<SelectUnitSetsDto, ListUnitSetsDto, CreateUnitSetsDto, UpdateUnitSetsDto, ListUnitSetsParameterDto>
    {
    }
}
