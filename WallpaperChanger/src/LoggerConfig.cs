
using NLog;
using NLog.Config;
using NLog.Targets;

namespace WallPal
{
    public static class LoggerConfig
    {
        public static void ConfigureNLog()
        {
            string logDir = DirectoryHelper.GetLogsDirectory();

            LoggingConfiguration config = new();

            string logLayout = "[${longdate}] [${level}] [${logger}] ${message}";

            FileTarget fileTarget = new("fileTarget")
            {
                FileName = Path.Combine(logDir, "${shortdate}.log"),
                Layout = logLayout,
                ArchiveFileName = Path.Combine(logDir, "archives", "log.{#}.log"),
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 7,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                Encoding = System.Text.Encoding.UTF8
            };
            config.AddTarget(fileTarget);

            LoggingRule fileRule = new("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(fileRule);

            ConsoleTarget consoleTarget = new("consoleTarget")
            {
                Layout = logLayout
            };
            config.AddTarget(consoleTarget);

            LoggingRule consoleRule = new("*", LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(consoleRule);

            LogManager.Configuration = config;
        }
    }
}
