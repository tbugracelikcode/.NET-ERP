using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    public interface IBranchesAppService : ICrudAppService<SelectBranchesDto, ListBranchesDto, CreateBranchesDto, UpdateBranchesDto,ListBranchesParameterDto>
    {
    }
}
