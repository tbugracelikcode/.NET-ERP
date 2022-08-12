using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    public interface IDeleteAppService : IApplicationService
    {
        Task DeleteAsync(Guid id);
    }
}
