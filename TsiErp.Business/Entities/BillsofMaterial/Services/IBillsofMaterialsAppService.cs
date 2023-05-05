using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    public interface IBillsofMaterialsAppService : ICrudAppService<SelectBillsofMaterialsDto, ListBillsofMaterialsDto, CreateBillsofMaterialsDto, UpdateBillsofMaterialsDto, ListBillsofMaterialsParameterDto>
    {

    }
}
