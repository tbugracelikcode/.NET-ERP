using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Validations
{
    public class CreateProductionTrackingsValidatorDto : TsiAbstractValidatorBase<CreateProductionTrackingsDto>
    {
        public CreateProductionTrackingsValidatorDto()
        {

        }
    }
}
