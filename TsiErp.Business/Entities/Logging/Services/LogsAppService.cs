using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Tsi.Core.Utilities.Guids;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Logging;
using TsiErp.Entities.Entities.Logging.Dtos;
using TsiErp.Entities.Entities.Logging;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using Newtonsoft.Json;
using TsiErp.DataAccess.Services.Login;

namespace TsiErp.Business.Entities.Logging.Services
{
    [ServiceRegistration(typeof(ILogsAppService), DependencyInjectionType.Scoped)]
    public class LogsAppService :  ILogsAppService
    {
        public CreateLogsDto CreateLog(object beforeValues, object afterValues, string logLevel, string methodName, Guid userId)
        {
            var log = new CreateLogsDto
            {
                AfterValues = JsonConvert.SerializeObject(afterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(beforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = LoginedUserService.UserId,
                LogLevel_ = logLevel,
                MethodName_ = methodName,
                UserId = userId
            };

            return log;
        }

        public async Task InsertAsync(object beforeValues, object afterValues, string logLevel, string methodName, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var createdLogObject = CreateLog(beforeValues, afterValues, logLevel, methodName, userId);

                var logRecord = new Logs()
                {
                    AfterValues = createdLogObject.AfterValues,
                    BeforeValues = createdLogObject.BeforeValues,
                    Date_ = createdLogObject.Date_,
                    LogLevel_ = createdLogObject.LogLevel_,
                    MethodName_ = createdLogObject.MethodName_,
                    UserId = createdLogObject.UserId
                };

                await _uow.LogsRepository.InsertAsync(logRecord);
                await _uow.SaveChanges();
            }
        }
    }
}
