using Interfaces;
using NLog;

namespace WallPal
{
    class Program
    {
        private static WallpaperChanger _wallpaperChanger = new();

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            LoggerConfig.ConfigureNLog();

            while (true)
            {
                ChangeWallpaper();

                int? refreshInterval = ConfigManager.GetConfigIntValue("refreshInterval");
                if (refreshInterval == null)
                {
                    _logger.Warn("No refresh interval specified in config file. Using default value of 5 minutes.");
                    refreshInterval = 5 * 60;
                }
                else
                {
                    _logger.Info($"Next wallpaper change in {refreshInterval} seconds.");
                }

                await Task.Delay(refreshInterval.Value * 1000);
            }
        }

        public static void ChangeWallpaper()
        {
            string pluginDirectory = DirectoryHelper.GetPluginsDirectory();
            PluginManager pluginManager = new(pluginDirectory);

            string? source = ConfigManager.GetConfigValue("source");
            if (source == null)
            {
                _logger.Error("No source specified in config file.");
                return;
            }

            IPlugin? plugin = pluginManager.GetPlugin(source);
            if (plugin != null)
            {
                if (plugin.IsReady())
                {

                    using Stream imageStream = plugin.GetWallpaperStream();
                    if (imageStream != null)
                    {
                        _logger.Info($"Changing wallpaper using plugin: {plugin.Name}");
                        _wallpaperChanger.ChangeWallpaper(imageStream);
                    }
                    else
                    {
                        _logger.Error("Plugin did not provide an image stream.");
                    }
                }
                else
                {
                    _logger.Error("Plugin is not ready.");
                }
            }
            else
            {
                _logger.Error($"Plugin {source} not found.");
            }
        }
    }
}

