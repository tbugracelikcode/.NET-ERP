using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionTracking;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.ProductionTracking;

namespace TsiErp.Business.Entities.ProductionTracking.BusinessRules
{
    public class ProductionTrackingManager
    {
        public async Task CodeControl(IProductionTrackingsRepository _repository)
        {
        }

        public async Task UpdateControl(IProductionTrackingsRepository _repository, Guid id, ProductionTrackings entity)
        {
        }

        public async Task DeleteControl(IProductionTrackingsRepository _repository, Guid id)
        {
        }
    }
}
