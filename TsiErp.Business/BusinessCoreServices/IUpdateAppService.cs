using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;

namespace TsiErp.Business.BusinessCoreServices
{
    public interface IUpdateAppService<TGetOutputDto, in TUpdateInput> : IApplicationService
    {
        Task<IDataResult<TGetOutputDto>> UpdateAsync(TUpdateInput input);
    }
}
