using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;

namespace TsiErp.Business.Entities.PackageFiche.Services
{
    public interface IPackageFichesAppService : ICrudAppService<SelectPackageFichesDto, ListPackageFichesDto, CreatePackageFichesDto, UpdatePackageFichesDto, ListPackageFichesParameterDto>
    {

        Task<IDataResult<IList<SelectPackageFichesDto>>> GetSelectListbyCurrentAccountandPackageTypeAsync(Guid currentAccountID, string packageType);
    }
}
