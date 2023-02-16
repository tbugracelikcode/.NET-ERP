using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    public interface IBillsofMaterialsAppService : ICrudAppService<SelectBillsofMaterialsDto, ListBillsofMaterialsDto, CreateBillsofMaterialsDto, UpdateBillsofMaterialsDto, ListBillsofMaterialsParameterDto>
    {
        Task<IDataResult<SelectBillsofMaterialsDto>> GetSelectListAsync(Guid finishedproductId);

    }
}
