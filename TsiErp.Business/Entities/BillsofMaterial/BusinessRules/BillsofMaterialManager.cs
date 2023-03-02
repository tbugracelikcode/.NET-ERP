using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Localizations.Resources.BillsofMaterials.Page;

namespace TsiErp.Business.Entities.BillsofMaterial.BusinessRules
{
    public class BillsofMaterialManager
    {
        public async Task CodeControl(IBillsofMaterialsRepository _repository, string code, IStringLocalizer<BillsofMaterialsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IBillsofMaterialsRepository _repository, string code, Guid id, BillsofMaterials entity, IStringLocalizer<BillsofMaterialsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
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
