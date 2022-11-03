using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Warehouse;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.WareHouse;

namespace TsiErp.Business.Entities.Warehouse.BusinessRules
{
    public class WarehouseManager
    {
        public async Task CodeControl(IWarehousesRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IWarehousesRepository _repository, string code, Guid id, Warehouses entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IWarehousesRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.SalesPropositions.Any(x => x.BranchID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
