using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationInventory;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.StationInventory;

namespace TsiErp.Business.Entities.StationInventory.BusinessRules
{
    public class StationInventoryManager
    {
        public async Task ProductControl(IStationInventoriesRepository _repository, Guid productID, Guid stationID)
        {
            if (await _repository.AnyAsync(t => t.ProductID == productID && t.StationID == stationID))
            {
                throw new DuplicateCodeException("Aynı stok kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateProductControl(IStationInventoriesRepository _repository, Guid productID, Guid stationID, Guid id)
        {
            if (await _repository.AnyAsync(t => t.ProductID == productID && t.StationID == stationID && t.Id !=id))
            {
                throw new DuplicateCodeException("Aynı stok kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IStationInventoriesRepository _repository, Guid id)
        {
        }
    }
}
