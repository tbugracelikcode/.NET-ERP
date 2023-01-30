using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractProductionTracking;
using TsiErp.Entities.Entities.ContractProductionTracking;

namespace TsiErp.Business.Entities.ContractProductionTracking.BusinessRules
{
    public class ContractProductionTrackingManager
    {
        public async Task CodeControl(IContractProductionTrackingsRepository _repository)
        {
        }

        public async Task UpdateControl(IContractProductionTrackingsRepository _repository, Guid id, ContractProductionTrackings entity)
        {
        }

        public async Task DeleteControl(IContractProductionTrackingsRepository _repository, Guid id)
        {
        }
    }
}
