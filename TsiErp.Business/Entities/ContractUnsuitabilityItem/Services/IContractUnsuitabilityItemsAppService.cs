using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.ContractUnsuitabilityItem.Services
{
    public interface IContractUnsuitabilityItemsAppService : ICrudAppService<SelectContractUnsuitabilityItemsDto, ListContractUnsuitabilityItemsDto, CreateContractUnsuitabilityItemsDto, UpdateContractUnsuitabilityItemsDto, ListContractUnsuitabilityItemsParameterDto>
    {
    }
}
