using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    public interface IUpdateAppService<TGetOutputDto, in TUpdateInput> : IApplicationService
    {
        Task<TGetOutputDto> UpdateAsync(TUpdateInput input);
    }
}
