using Interfaces;
using Newtonsoft.Json.Linq;
using NLog;

namespace WallPal
{
    public class ApplicationServices(string pluginName) : IApplicationServices
    {
        private readonly string _pluginName = pluginName;

        public string GetAppDirectory()
        {
            return DirectoryHelper.GetAppDirectory();
        }

        public JObject GetConfig(JObject defaultConfig)
        {
            return ConfigManager.GetPluginConfig(_pluginName, defaultConfig);
        }

        public Logger GetLogger(string name)
        {
            string loggerName = $"Plugin.{_pluginName}.{name}";
            return LogManager.GetLogger(loggerName);
        }

        public Logger GetLogger()
        {
            string loggerName = $"Plugin.{_pluginName}";
            return LogManager.GetLogger(loggerName);
        }
    }
}
