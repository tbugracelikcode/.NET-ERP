using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;

namespace TsiErp.Business.Entities.ProductionTrackingHaltLine.Validations
{
    public class UpdateProductionTrackingHaltLinesValidator : TsiAbstractValidatorBase<UpdateProductionTrackingHaltLinesDto>
    {
        public UpdateProductionTrackingHaltLinesValidator()
        {

        }
    }
}
