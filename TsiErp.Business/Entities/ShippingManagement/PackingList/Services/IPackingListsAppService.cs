using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;

namespace TsiErp.Business.Entities.PackingList.Services
{
    public interface IPackingListsAppService : ICrudAppService<SelectPackingListsDto, ListPackingListsDto, CreatePackingListsDto, UpdatePackingListsDto, ListPackingListsParameterDto>
    {
        Task<IResult> DeleteLineCubageAsync(Guid id);
        Task<IResult> DeleteLinePalletAsync(Guid id);
        Task<IResult> DeleteLinePalletPackageAsync(Guid id);
        Task<IDataResult<IList<SelectPackingListPalletPackageLinesDto>>> GetLinePalletPackageListAsync();
        Task<IDataResult<IList<SelectPackingListPalletPackageLinesDto>>> GetPackingListLineByPackageId(Guid PackageFicheId);
    }
}
