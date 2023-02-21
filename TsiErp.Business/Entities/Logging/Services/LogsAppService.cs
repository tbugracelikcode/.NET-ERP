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

        public static Logs InsertLogToDatabase<TSource, TDestination>(TSource before, TSource after, Guid userId, string logLevel, LogType logType, Guid recordId)
        {

            Logs log = new Logs();

            switch (logType)
            {
                case LogType.Insert:
                    var beforeInsertEntity = ObjectMapper.Map<TSource, TDestination>(before);

                    var afterInsertEntity = ObjectMapper.Map<TSource, TDestination>(after);

                    log = new Logs
                    {
                        AfterValues = JsonConvert.SerializeObject(afterInsertEntity, Formatting.Indented),
                        BeforeValues = JsonConvert.SerializeObject(beforeInsertEntity, Formatting.Indented),
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId
                    };
                    break;
                case LogType.Update:
                    var beforeUpdateEntity = ObjectMapper.Map<TSource, TDestination>(before);

                    var afterUpdateEntity = ObjectMapper.Map<TSource, TDestination>(after);

                    log = new Logs
                    {
                        AfterValues = JsonConvert.SerializeObject(afterUpdateEntity, Formatting.Indented),
                        BeforeValues = JsonConvert.SerializeObject(beforeUpdateEntity, Formatting.Indented),
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId
                    };
                    break;
                case LogType.Delete:

                    log = new Logs
                    {
                        AfterValues = recordId,
                        BeforeValues = recordId,
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId
                    };
                    break;
                case LogType.Get:
                    var beforeGetEntity = ObjectMapper.Map<TSource, TDestination>(before);

                    var afterGetEntity = ObjectMapper.Map<TSource, TDestination>(after);

                    log = new Logs
                    {
                        AfterValues = JsonConvert.SerializeObject(afterGetEntity, Formatting.Indented),
                        BeforeValues = JsonConvert.SerializeObject(beforeGetEntity, Formatting.Indented),
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId
                    };
                    break;
                default:
                    break;
            }

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
