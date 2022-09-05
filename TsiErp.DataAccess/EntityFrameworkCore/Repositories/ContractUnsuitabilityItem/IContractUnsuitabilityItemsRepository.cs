using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractUnsuitabilityItem
{
    public interface IContractUnsuitabilityItemsRepository : IEfCoreRepository<ContractUnsuitabilityItems>
    {
    }
}
