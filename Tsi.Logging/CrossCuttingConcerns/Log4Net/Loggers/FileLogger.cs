using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Logging.CrossCuttingConcerns.Log4Net.Loggers
{
    public class FileLogger : LoggerServiceBase
    {
        public FileLogger() : base("JsonFileLogger")
        {
        }
    }
}
