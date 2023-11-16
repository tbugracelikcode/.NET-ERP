using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;

namespace TsiErp.Business.Entities.PackageFiche.Services
{
    public interface IPackageFichesAppService : ICrudAppService<SelectPackageFichesDto, ListPackageFichesDto, CreatePackageFichesDto, UpdatePackageFichesDto, ListPackageFichesParameterDto>
    {
    }
}
