using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;

namespace Tsi.Core.Services.BusinessCoreServices
{
    public interface IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, in TGetListInput>
    {
        Task<IDataResult<TGetOutputDto>> GetAsync(Guid id);

        Task<IDataResult<IList<TGetListOutputDto>>> GetListAsync(TGetListInput input);
    }
}
