using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Guids;
using Tsi.IoC.Tsi.DependencyResolvers;
using Tsi.Logging.EntityFrameworkCore.Entities;
using Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.Logging.Tsi.Dtos;

namespace Tsi.Logging.Tsi.Services
{
    [ServiceRegistration(typeof(ILogsAppService), DependencyInjectionType.Scoped)]
    public class LogsAppService :ApplicationService, ILogsAppService
    {
        private readonly ILogsRepository _repository;

        public LogsAppService(ILogsRepository repository)
        {
            _repository = repository;
        }

        public async Task InsertAsync(CreateLogsDto input)
        {
            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(input.AfterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(input.BeforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = GuidGenerator.CreateGuid(),
                LogLevel_ = input.LogLevel_,
                MethodName_ = input.MethodName_,
                UserId = input.UserId
            };

            await _repository.InsertAsync(log);
        }
    }
}
