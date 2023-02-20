using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;

namespace TsiErp.Business.BusinessCoreServices
{
    public interface IDataConcurrencyStampUpdate<TGetOutputDto> : IApplicationService
    {
        Task<IDataResult<TGetOutputDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow,Guid userId);
    }
}
