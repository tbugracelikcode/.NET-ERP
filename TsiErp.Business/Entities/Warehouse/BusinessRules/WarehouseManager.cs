using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Warehouse;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Localizations.Resources.Warehouses.Page;

namespace TsiErp.Business.Entities.Warehouse.BusinessRules
{
    public class WarehouseManager
    {
        public async Task CodeControl(IWarehousesRepository _repository, string code, IStringLocalizer<WarehousesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IWarehousesRepository _repository, string code, Guid id, Warehouses entity, IStringLocalizer<WarehousesResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IWarehousesRepository _repository, Guid id, IStringLocalizer<WarehousesResource> L)
        {
            //if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.BranchID == id)))
            //{
            //    throw new Exception(L["DeleteControlManager"]);
            //}
        }
    }
}
