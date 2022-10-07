using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductGroup;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.ProductGroup;

namespace TsiErp.Business.Entities.ProductGroup.BusinessRules
{
    public class ProductGroupManager
    {
        public async Task CodeControl(IProductGroupsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IProductGroupsRepository _repository, string code, Guid id, ProductGroups entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IProductGroupsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.Products.Any(x => x.ProductGrpID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
