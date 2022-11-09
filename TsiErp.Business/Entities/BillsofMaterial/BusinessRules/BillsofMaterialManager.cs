using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterial;

namespace TsiErp.Business.Entities.BillsofMaterial.BusinessRules
{
    public class BillsofMaterialManager
    {
        public async Task CodeControl(IBillsofMaterialsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IBillsofMaterialsRepository _repository, string code, Guid id, BillsofMaterials entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IBillsofMaterialsRepository _repository, Guid id, Guid lineId, bool lineDelete)
        {
            if (lineDelete)
            {
                var entity = await _repository.GetAsync(t => t.Id == id, t => t.BillsofMaterialLines);

                var line = entity.BillsofMaterialLines.Where(t => t.Id == lineId).FirstOrDefault();

            }
            else
            {
                var entity = await _repository.GetAsync(t => t.Id == id);

            }
        }
    }
}
