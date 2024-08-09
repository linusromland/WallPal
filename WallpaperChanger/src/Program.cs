using Interfaces;

namespace WallPal
{
    class Program
    {
        private static WallpaperChanger _wallpaperChanger = new();

        static async Task Main(string[] args)
        {
            while (true)
            {
                ChangeWallpaper();

                int? refreshInterval = ConfigManager.GetConfigIntValue("refreshInterval");
                if (refreshInterval == null)
                {
                    Console.WriteLine("No refresh interval specified in config file. Using default value of 5 minutes.");
                    refreshInterval = 5 * 60;
                }
                else
                {
                    Console.WriteLine($"Next wallpaper change in {refreshInterval} seconds.");
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
                Console.WriteLine("No source specified in config file.");
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
                        Console.WriteLine($"Changing wallpaper using plugin: {plugin.Name}");
                        _wallpaperChanger.ChangeWallpaper(imageStream);
                    }
                    else
                    {
                        Console.WriteLine("Plugin did not provide an image stream.");
                    }
                }
                else
                {
                    Console.WriteLine("Plugin is not ready.");
                }
            }
            else
            {
                Console.WriteLine($"Plugin {source} not found.");
            }
        }
    }
}

