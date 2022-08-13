using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsi.Results;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    public interface IDeleteAppService : IApplicationService
    {
        Task<IResult> DeleteAsync(Guid id);
    }
}
