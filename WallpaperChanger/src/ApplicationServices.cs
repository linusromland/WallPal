using Interfaces;
using Newtonsoft.Json.Linq;

namespace WallPal
{
    public class ApplicationServices : IApplicationServices
    {
        private readonly string _pluginName;

        public ApplicationServices(string pluginName)
        {
            _pluginName = pluginName;
        }

        public string GetAppDirectory()
        {
            return DirectoryHelper.GetAppDirectory();
        }

        public JObject GetConfig(JObject defaultConfig)
        {
            return ConfigManager.GetPluginConfig(_pluginName, defaultConfig);
        }
    }
}
