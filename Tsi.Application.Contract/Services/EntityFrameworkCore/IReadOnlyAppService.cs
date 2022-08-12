using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    public interface IReadOnlyAppService<TGetOutputDto, TGetListOutputDto>
    {
        Task<TGetOutputDto> GetAsync(Guid id);

        Task<IList<TGetListOutputDto>> GetListAsync();
    }
}
