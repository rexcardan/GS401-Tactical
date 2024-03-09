using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourChecker
{
    public class LogHelper
    {
        public static void SetupLogging()
        {
            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console
            var logfile = new FileTarget("logfile")
            {
                FileName = "${basedir}/AppData/Logs/log.txt",
                Layout = "${longdate} ${level} ${message}  ${exception} ${event-properties:myProperty}"
            };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }
    }
}
