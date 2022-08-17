using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Guids;
using Tsi.Logging.EntityFrameworkCore.Entities;
using Tsi.Logging.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.Logging.Tsi.Dtos;

namespace Tsi.Logging.Tsi.Services
{
    public class LogsAppService : ILogsAppService
    {
        private readonly ILogsRepository _repository;

        private readonly IGuidGenerator _guidGenerator;

        public LogsAppService(ILogsRepository repository, IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }

        public async Task InsertAsync(CreateLogsDto input)
        {
            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(input.AfterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(input.BeforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = _guidGenerator.CreateGuid(),
                LogLevel_ = input.LogLevel_,
                MethodName_ = input.MethodName_,
                UserId = input.UserId
            };

            await _repository.InsertAsync(log);
        }
    }
}
