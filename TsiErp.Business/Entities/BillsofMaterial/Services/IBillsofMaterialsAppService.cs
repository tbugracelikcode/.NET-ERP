using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    public interface IBillsofMaterialsAppService : ICrudAppService<SelectBillsofMaterialsDto, ListBillsofMaterialsDto, CreateBillsofMaterialsDto, UpdateBillsofMaterialsDto, ListBillsofMaterialsParameterDto>
    {
    }
}
