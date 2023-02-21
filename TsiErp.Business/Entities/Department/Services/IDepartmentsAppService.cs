using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.Department.Dtos;

namespace TsiErp.Business.Entities.Department.Services
{
    public interface IDepartmentsAppService : ICrudAppService<SelectDepartmentsDto, ListDepartmentsDto, CreateDepartmentsDto, UpdateDepartmentsDto, ListDepartmentsParameterDto>
    {
    }
}
