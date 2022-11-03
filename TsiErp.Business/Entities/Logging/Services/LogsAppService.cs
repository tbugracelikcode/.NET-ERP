using Newtonsoft.Json;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Tsi.Core.Utilities.Guids;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Logging;
using TsiErp.Entities.Entities.Logging.Dtos;
using TsiErp.Entities.Entities.Logging;
using Tsi.Application.Contract.Services.EntityFrameworkCore;

namespace TsiErp.Business.Entities.Logging.Services
{
    [ServiceRegistration(typeof(ILogsAppService), DependencyInjectionType.Scoped)]
    public class LogsAppService : ApplicationService, ILogsAppService
    {
        private readonly ILogsRepository _repository;

        public LogsAppService(ILogsRepository repository)
        {
            _repository = repository;
        }

        public CreateLogsDto CreateLog(object beforeValues, object afterValues, string logLevel, string methodName, Guid userId)
        {
            var log = new CreateLogsDto
            {
                AfterValues = JsonConvert.SerializeObject(afterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(beforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = GuidGenerator.CreateGuid(),
                LogLevel_ = logLevel,
                MethodName_ = methodName,
                UserId = userId
            };

            return log;
        }

        public async Task InsertAsync(object beforeValues, object afterValues, string logLevel, string methodName, Guid userId)
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

            await _repository.InsertAsync(logRecord);
            await _repository.SaveChanges();
        }
    }
}
