using JsonDiffer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Tsi.Core.Utilities.Guids;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Entities.Entities.Other.Logging;
using TsiErp.Entities.Entities.Other.Logging.Dtos;
using TsiErp.Entities.TableConstant;

namespace TsiErp.Business.Entities.Logging.Services
{

    public static class LogsAppService
    {
        static QueryFactory queryFactory { get; set; } = new QueryFactory();

        public static IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();

        static string MainRootName = "AppLogs";

        private static IGetSQLDateAppService _GetSQLDateAppService { get; set; } = new GetSQLDateAppService();

        public static void InsertLogToDatabase(object before, object after, Guid userId, string logLevel, LogType logType, Guid recordId)
        {
            switch (logType)
            {
                case LogType.Insert:

                    string rootPathIns = GetInsertRootPath(logLevel, recordId);

                    CreateLogsDto insertDto = new CreateLogsDto()
                    {
                        UserId = userId,
                        RecordId = recordId,
                        Date_ = _GetSQLDateAppService.GetDateFromSQL(),
                        DiffValues = "",
                        AfterValues = "",
                        BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                    };

                    var jsonInsertLogFileText = JsonConvert.SerializeObject(insertDto, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    if (!Directory.Exists(rootPathIns))
                    {
                        Directory.CreateDirectory(rootPathIns);
                    }

                    string insertFileName = insertDto.Date_.Hour.ToString() + insertDto.Date_.Minute.ToString() + insertDto.Date_.Second.ToString() + ".json";

                    rootPathIns = Path.Combine(rootPathIns, insertFileName);

                    if (!File.Exists(rootPathIns))
                    {
                        File.Create(rootPathIns).Close();
                    }

                    FileStream fsInsert = new FileStream(rootPathIns, FileMode.Open, FileAccess.Write);
                    StreamWriter swInsert = new StreamWriter(fsInsert);

                    swInsert.WriteLine(jsonInsertLogFileText);


                    swInsert.Flush();
                    swInsert.Close();
                    fsInsert.Close();

                    break;
                case LogType.Update:

                    string rootPathUpd = GetUpdateRootPath(logLevel, recordId, userId);

                    var j1 = JToken.Parse(JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                    var j2 = JToken.Parse(JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                    var diff = JsonConvert.SerializeObject(JsonDifferentiator.Differentiate(j1, j2, OutputMode.Symbol, showOriginalValues: false));

                    CreateLogsDto updateDto = new CreateLogsDto()
                    {
                        UserId = userId,
                        RecordId = recordId,
                        Date_ = _GetSQLDateAppService.GetDateFromSQL(),
                        DiffValues = diff,
                        AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
                    };

                    var jsonUpdateLogFileText = JsonConvert.SerializeObject(updateDto, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    if (!Directory.Exists(rootPathUpd))
                    {
                        Directory.CreateDirectory(rootPathUpd);
                    }

                    string updateFileName = updateDto.Date_.Hour.ToString() + updateDto.Date_.Minute.ToString() + updateDto.Date_.Second.ToString() + ".json";

                    rootPathUpd = Path.Combine(rootPathUpd, updateFileName);

                    if (!File.Exists(rootPathUpd))
                    {
                        File.Create(rootPathUpd).Close();
                    }

                    FileStream fsUpdate = new FileStream(rootPathUpd, FileMode.Open, FileAccess.Write);
                    StreamWriter swUpdate = new StreamWriter(fsUpdate);

                    swUpdate.WriteLine(jsonUpdateLogFileText);


                    swUpdate.Flush();
                    swUpdate.Close();
                    fsUpdate.Close();

                    break;
                case LogType.Delete:

                    string rootPathDel = GetDeleteRootPath(logLevel, recordId);

                    CreateLogsDto deleteDto = new CreateLogsDto()
                    {
                        UserId = userId,
                        RecordId = recordId,
                        Date_ = _GetSQLDateAppService.GetDateFromSQL(),
                        DiffValues = "",
                        AfterValues = recordId,
                        BeforeValues = recordId
                    };

                    var jsonDeleteLogFileText = JsonConvert.SerializeObject(deleteDto, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    if (!Directory.Exists(rootPathDel))
                    {
                        Directory.CreateDirectory(rootPathDel);
                    }

                    string deleteFileName = deleteDto.Date_.Hour.ToString() + deleteDto.Date_.Minute.ToString() + deleteDto.Date_.Second.ToString() + ".json";

                    rootPathDel = Path.Combine(rootPathDel, deleteFileName);

                    if (!File.Exists(rootPathDel))
                    {
                        File.Create(rootPathDel).Close();
                    }

                    FileStream fsDelete = new FileStream(rootPathDel, FileMode.Open, FileAccess.Write);
                    StreamWriter swDelete = new StreamWriter(fsDelete);

                    swDelete.WriteLine(jsonDeleteLogFileText);


                    swDelete.Flush();
                    swDelete.Close();
                    fsDelete.Close();
                    break;
                case LogType.Get:

                    string rootPathGet = GetGetRootPath(logLevel, recordId);

                    string getLogValue = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    //var logGetQuery = queryFactory.Query().From(Tables.Logs).Insert(new CreateLogsDto
                    //{
                    //    AfterValues = JsonConvert.SerializeObject(after, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    //    BeforeValues = JsonConvert.SerializeObject(before, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    //    Date_ = _GetSQLDateAppService.GetDateFromSQL(),
                    //    Id = GuidGenerator.CreateGuid(),
                    //    LogLevel_ = logLevel,
                    //    MethodName_ = LogType.Get.GetType().GetEnumName(LogType.Get),
                    //    UserId = userId,
                    //    RecordId = recordId,
                    //    DiffValues = ""
                    //});

                    //var GetLog = queryFactory.Insert<CreateLogsDto>(logGetQuery, "Id", true);


                    break;
                default:
                    break;
            }
        }

        public static string GetInsertRootPath(string logLevel, Guid recordId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string rootPath = Path.Combine(basePath, MainRootName);

            rootPath = Path.Combine(rootPath, logLevel, "Insert", recordId.ToString(), _GetSQLDateAppService.GetDateFromSQL().Date.ToShortDateString());

            return rootPath;
        }

        public static string GetDeleteRootPath(string logLevel, Guid recordId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string rootPath = Path.Combine(basePath, MainRootName);

            rootPath = Path.Combine(rootPath, logLevel, "Delete", recordId.ToString(), _GetSQLDateAppService.GetDateFromSQL().Date.ToShortDateString());

            return rootPath;
        }

        public static string GetGetRootPath(string logLevel, Guid recordId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string rootPath = Path.Combine(basePath, MainRootName);

            rootPath = Path.Combine(rootPath, logLevel, "Get", recordId.ToString(), _GetSQLDateAppService.GetDateFromSQL().Date.ToShortDateString());

            return rootPath;
        }

        public static string GetUpdateRootPath(string logLevel, Guid recordId, Guid userId)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string rootPath = Path.Combine(basePath, MainRootName);

            rootPath = Path.Combine(rootPath, logLevel, "Update", recordId.ToString(), _GetSQLDateAppService.GetDateFromSQL().Date.ToShortDateString(), userId.ToString());

            return rootPath;
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
