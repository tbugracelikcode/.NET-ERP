using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;

namespace TsiErp.Business.Entities.ProductionTrackingHaltLine.BusinessRules
{
    public class ProductionTrackingHaltLineManager
    {
        public async Task CodeControl(IProductionTrackingHaltLinesRepository _repository)
        {
        }

        public async Task UpdateControl(IProductionTrackingHaltLinesRepository _repository, Guid id, ProductionTrackingHaltLines entity)
        {
        }

        public async Task DeleteControl(IProductionTrackingHaltLinesRepository _repository, Guid id)
        {
        }
    }
}
