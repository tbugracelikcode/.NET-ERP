using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.CrossCuttingConcerns.Validation;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ContractProductionTracking.Validations
{
    public class UpdateContractProductionTrackingsValidator : TsiAbstractValidatorBase<UpdateContractProductionTrackingsDto>
    {
        public UpdateContractProductionTrackingsValidator()
        {

        }
    }
}
