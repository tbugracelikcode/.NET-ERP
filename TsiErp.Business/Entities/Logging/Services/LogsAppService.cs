using JsonDiffer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tsi.Core.Utilities.Guids;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Logging;
using TsiErp.Entities.Entities.Logging.Dtos;
using TsiErp.Entities.TableConstant;

namespace TsiErp.Business.Entities.Logging.Services
{

    public static class LogsAppService
    {
        static QueryFactory queryFactory { get; set; } = new QueryFactory();

        public static IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();

        public static Logs InsertLogToDatabase(object before, object after, Guid userId, string logLevel, LogType logType, Guid recordId)
        {

            Logs log = new Logs();

            switch (logType)
            {
                case LogType.Insert:
                    using (var connection = queryFactory.ConnectToDatabase())
                    {
                        var logInsertQuery = queryFactory.Query().From(Tables.Logs).Insert(new CreateLogsDto
                        {
                            AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                            BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                            Date_ = DateTime.Now,
                            Id = GuidGenerator.CreateGuid(),
                            LogLevel_ = "Branches",
                            MethodName_ = LogType.Insert.GetType().GetEnumName(LogType.Insert),
                            UserId = userId,
                            RecordId = recordId,
                            DiffValues = ""
                        });

                        var InsertLog = queryFactory.Insert<CreateLogsDto>(logInsertQuery, "Id", true);
                    }
                    

                    break;
                case LogType.Update:

                    using (var connection = queryFactory.ConnectToDatabase())
                    {
                        var j1 = JToken.Parse(JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                        var j2 = JToken.Parse(JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                        var diff = JsonConvert.SerializeObject(JsonDifferentiator.Differentiate(j1, j2, OutputMode.Symbol, showOriginalValues: false));

                        var logUpdateQuery = queryFactory.Query().From(Tables.Logs).Insert(new CreateLogsDto
                        {
                            AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                            BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                            Date_ = DateTime.Now,
                            Id = GuidGenerator.CreateGuid(),
                            LogLevel_ = "Branches",
                            MethodName_ = LogType.Update.GetType().GetEnumName(LogType.Update),
                            UserId = userId,
                            RecordId = recordId,
                            DiffValues = diff
                        });

                        var UpdateLog = queryFactory.Insert<CreateLogsDto>(logUpdateQuery, "Id", true);

                    }
                    break;
                case LogType.Delete:

                    using (var connection = queryFactory.ConnectToDatabase())
                    {

                        var logDeleteQuery = queryFactory.Query().From(Tables.Logs).Insert(new CreateLogsDto
                        {
                            AfterValues = recordId,
                            BeforeValues = recordId,
                            Date_ = DateTime.Now,
                            Id = GuidGenerator.CreateGuid(),
                            LogLevel_ = "Branches",
                            MethodName_ = LogType.Delete.GetType().GetEnumName(LogType.Delete),
                            UserId = userId,
                            RecordId = recordId,
                            DiffValues = ""
                        });

                        var DeleteLog = queryFactory.Insert<CreateLogsDto>(logDeleteQuery, "Id", true);

                    }

                    break;
                case LogType.Get:

                    using (var connection = queryFactory.ConnectToDatabase())
                    {
                        var logGetQuery = queryFactory.Query().From(Tables.Logs).Insert(new CreateLogsDto
                        {
                            AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                            BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                            Date_ = DateTime.Now,
                            Id = GuidGenerator.CreateGuid(),
                            LogLevel_ = "Branches",
                            MethodName_ = LogType.Get.GetType().GetEnumName(LogType.Get),
                            UserId = userId,
                            RecordId = recordId,
                            DiffValues = ""
                        });

                        var GetLog = queryFactory.Insert<CreateLogsDto>(logGetQuery, "Id", true);

                    }
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
