using Microsoft.Extensions.Localization;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ContractProductionTracking;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Localizations.Resources.ContractProductionTrackings.Page;

namespace TsiErp.Business.Entities.ContractProductionTracking.BusinessRules
{
    public class ContractProductionTrackingManager
    {
        public async Task CodeControl(IContractProductionTrackingsRepository _repository, IStringLocalizer<ContractProductionTrackingsResource> L)
        {
        }

        public async Task UpdateControl(IContractProductionTrackingsRepository _repository, Guid id, ContractProductionTrackings entity, IStringLocalizer<ContractProductionTrackingsResource> L)
        {
        }

        public async Task DeleteControl(IContractProductionTrackingsRepository _repository, Guid id)
        {
        }
    }
}
