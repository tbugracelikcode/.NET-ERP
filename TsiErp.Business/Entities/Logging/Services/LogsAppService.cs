using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Logging;
using TsiErp.Entities.Entities.Logging.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;

namespace TsiErp.Business.Entities.Logging.Services
{

    public static class LogsAppService
    {
        public static Logs CreateLogObject(object beforeValues, object afterValues, Guid userId, string logLevel, Guid recordId)
        {
            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(afterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(beforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = LoginedUserService.UserId,
                LogLevel_ = logLevel,
                MethodName_ = "Insert",
                UserId = userId,
                RecordId = recordId
            };

            return log;
        }

        public static Logs UpdateLogObject(object beforeValues, object afterValues, Guid userId, string logLevel, Guid recordId)
        {
            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(afterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(beforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = LoginedUserService.UserId,
                LogLevel_ = logLevel,
                MethodName_ = "Update",
                UserId = userId,
                RecordId = recordId
            };

            return log;
        }

        public static Logs DeleteLogObject(object beforeValues, object afterValues, Guid userId, string logLevel, Guid recordId)
        {
            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(afterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(beforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = LoginedUserService.UserId,
                LogLevel_ = logLevel,
                MethodName_ = "Delete",
                UserId = userId,
                RecordId = recordId
            };

            return log;
        }

        public static Logs GetLogObject(object beforeValues, object afterValues, Guid userId, string logLevel, Guid recordId)
        {
            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(afterValues, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(beforeValues, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = LoginedUserService.UserId,
                LogLevel_ = logLevel,
                MethodName_ = "Get",
                UserId = userId,
                RecordId = recordId
            };

            return log;
        }

        public static Logs InsertLogToDatabase<TSource, TDestination>(this TSource input, Guid userId, string logLevel, LogType logType, Guid recordId)
        {
            var mappedEntity = ObjectMapper.Map<TSource, TDestination>(input);

            var log = new Logs
            {
                AfterValues = JsonConvert.SerializeObject(input, Formatting.Indented),
                BeforeValues = JsonConvert.SerializeObject(mappedEntity, Formatting.Indented),
                Date_ = DateTime.Now,
                Id = LoginedUserService.UserId,
                LogLevel_ = logLevel,
                MethodName_ = logType.GetType().GetEnumName(logType),
                UserId = userId,
                RecordId = recordId
            };

            return log;
        }
    }

    public enum LogType
    {
        Insert,
        Update,
        Delete,
        Get
    }
}
