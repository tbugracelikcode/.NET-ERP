using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    public interface IBillsofMaterialsAppService : ICrudAppService<SelectBillsofMaterialsDto, ListBillsofMaterialsDto, CreateBillsofMaterialsDto, UpdateBillsofMaterialsDto, ListBillsofMaterialsParameterDto>
    {
        Task<IDataResult<SelectBillsofMaterialsDto>> GetSelectListAsync(Guid finishedproductId);

    }
}
