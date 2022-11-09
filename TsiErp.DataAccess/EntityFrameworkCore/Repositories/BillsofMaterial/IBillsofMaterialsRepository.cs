using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.BillsofMaterial;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterial
{
    public interface IBillsofMaterialsRepository : IEfCoreRepository<BillsofMaterials>
    {
    }
}
