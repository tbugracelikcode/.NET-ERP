using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;

namespace Tsi.Core.Services.BusinessCoreServices
{
    public interface ICreateAppService<TGetOutputDto, in TCreateInput> : IApplicationService
    {
        Task<IDataResult<TGetOutputDto>> CreateAsync(TCreateInput input);
    }
}
