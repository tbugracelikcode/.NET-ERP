using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;

namespace Tsi.Core.Services.BusinessCoreServices
{
    public interface IDeleteAppService : IApplicationService
    {
        Task<IResult> DeleteAsync(Guid id);
    }
}
