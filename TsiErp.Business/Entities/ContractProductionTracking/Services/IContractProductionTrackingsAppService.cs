using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ContractProductionTracking.Services
{
    public interface IContractProductionTrackingsAppService : ICrudAppService<SelectContractProductionTrackingsDto, ListContractProductionTrackingsDto, CreateContractProductionTrackingsDto, UpdateContractProductionTrackingsDto, ListContractProductionTrackingsParameterDto>
    {
        Task<IDataResult<IList<SelectContractProductionTrackingsDto>>> GetSelectListAsync(Guid productId);

    }
}
