using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationInventory;
using TsiErp.Localizations.Resources.StationInventrories.Page;

namespace TsiErp.Business.Entities.StationInventory.BusinessRules
{
    public class StationInventoryManager
    {
        public async Task ProductControl(IStationInventoriesRepository _repository, Guid productID, Guid stationID, IStringLocalizer<StationInventroriesResource> L)
        {
            if (await _repository.AnyAsync(t => t.ProductID == productID && t.StationID == stationID))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateProductControl(IStationInventoriesRepository _repository, Guid productID, Guid stationID, Guid id, IStringLocalizer<StationInventroriesResource> L)
        {
            if (await _repository.AnyAsync(t => t.ProductID == productID && t.StationID == stationID && t.Id !=id))
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IStationInventoriesRepository _repository, Guid id)
        {
        }
    }
}
