using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WallPal
{
    public static class ConfigManager
    {
        private static JObject GetConfig()
        {
            string configPath = DirectoryHelper.GetConfigFilePath();

            if (!File.Exists(configPath))
            {
                JObject defaultConfig = new()
                {
                    { "refreshInterval", 5 },
                    { "source", "SamplePlugin" }, // TODO: To be changed to be per time and stuff
                    { "plugins", new JObject() }
                };
                File.WriteAllText(configPath, defaultConfig.ToString());
                return defaultConfig;
            }

            string fileContent = File.ReadAllText(configPath);

            try
            {
                return JObject.Parse(fileContent);
            }
            catch (JsonReaderException)
            {
                throw new Exception("Config file is not a valid JSON file.");
            }
        }

        public static JObject GetPluginConfig(string pluginName, JObject defaultConfig)
        {
            JObject config = GetConfig();
            JObject? pluginConfig = config["plugins"]?[pluginName]?.ToObject<JObject>();

            if (pluginConfig == null)
            {
                if (!config.ContainsKey("plugins"))
                {
                    config["plugins"] = new JObject();
                }

                // todo: implement a better way for this
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                config["plugins"][pluginName] = defaultConfig;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                File.WriteAllText(DirectoryHelper.GetConfigFilePath(), config.ToString());
                return defaultConfig;
            }
            return pluginConfig;
        }

        public static string? GetConfigValue(string key)
        {
            JObject config = GetConfig();
            return config[key]?.ToString();
        }

        public static int? GetConfigIntValue(string key)
        {
            JObject config = GetConfig();
            return config[key]?.ToObject<int>();
        }
    }
}
