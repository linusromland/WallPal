
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

            FileTarget fileTarget = new FileTarget("fileTarget")
            {
                FileName = Path.Combine(logDir, "${shortdate}.log"),
                Layout = "${longdate} [${logger}] ${message}",
                ArchiveFileName = Path.Combine(logDir, "archives", "log.{#}.log"),
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 7,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                Encoding = System.Text.Encoding.UTF8
            };

            config.AddTarget(fileTarget);

            LoggingRule rule = new LoggingRule("*", LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
        }
    }
}
