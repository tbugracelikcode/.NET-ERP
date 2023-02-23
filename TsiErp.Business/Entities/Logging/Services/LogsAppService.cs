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
using Newtonsoft.Json.Linq;
using JsonDiffer;

namespace TsiErp.Business.Entities.Logging.Services
{

    public static class LogsAppService
    {

        public static Logs InsertLogToDatabase(object before, object after, Guid userId, string logLevel, LogType logType, Guid recordId)
        {

            Logs log = new Logs();

            switch (logType)
            {
                case LogType.Insert:

                    log = new Logs
                    {
                        AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId,
                        DiffValues = ""
                    };
                    break;
                case LogType.Update:

                    var j1 = JToken.Parse(JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    var j2 = JToken.Parse(JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                    var diff = JsonConvert.SerializeObject(JsonDifferentiator.Differentiate(j1, j2, OutputMode.Symbol, showOriginalValues: false));

                    log = new Logs
                    {
                        AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented,new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId,
                        DiffValues = diff
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
                        RecordId = recordId,
                        DiffValues = ""
                    };
                    break;
                case LogType.Get:
                    log = new Logs
                    {
                        AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        Date_ = DateTime.Now,
                        Id = LoginedUserService.UserId,
                        LogLevel_ = logLevel,
                        MethodName_ = logType.GetType().GetEnumName(logType),
                        UserId = userId,
                        RecordId = recordId,
                        DiffValues = ""
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
