using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Logging.Tsi.Dtos;

namespace Tsi.Logging.Tsi.Services
{
    public interface ILogsAppService
    {
        Task InsertAsync(CreateLogsDto input);
    }
}
