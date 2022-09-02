using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Logging.Dtos;

namespace TsiErp.Business.Entities.Logging.Services
{
    public interface ILogsAppService
    {
        Task InsertAsync(object beforeValues, object afterValues, string logLevel, string methodName, Guid userId);

        CreateLogsDto CreateLog(object beforeValues, object afterValues, string logLevel, string methodName, Guid userId);
    }
}
