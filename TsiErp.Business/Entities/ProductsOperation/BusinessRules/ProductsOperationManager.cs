using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperation;

namespace TsiErp.Business.Entities.ProductsOperation.BusinessRules
{
    public class ProductsOperationManager
    {
        public async Task CodeControl(IProductsOperationsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IProductsOperationsRepository _repository, string code, Guid id, ProductsOperations entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IProductsOperationsRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.ProductsOperationLines);

                var line = entity.ProductsOperationLines.Where(t => t.Id == lineId).FirstOrDefault();

            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

            }
        }
    }
}
