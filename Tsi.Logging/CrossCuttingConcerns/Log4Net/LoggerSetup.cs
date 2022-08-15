using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Text;
using Tsi.Logging.CrossCuttingConcerns.Log4Net.Layouts;

namespace Tsi.Logging.CrossCuttingConcerns.Log4Net
{
    public class LoggerSetup
    {
        public static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            

            FileAppender roller = new FileAppender();
            roller.AppendToFile = true;
            roller.File = @"Logs\EventLog.json";
            roller.Layout = new JsonLayout();
            
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }
    }
}
