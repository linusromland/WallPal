using System;
using Interfaces;

namespace WallPal
{
    class Program
    {
        static void Main(string[] args)
        {
            WallpaperChanger wallpaperChanger = new WallpaperChanger();


            string pluginDirectory = DirectoryHelper.GetPluginsDirectory();
            PluginManager pluginManager = new PluginManager(pluginDirectory);

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
                    wallpaperChanger.ChangeWallpaper(imageStream);
                }
                else
                {
                    Console.WriteLine("Plugin did not provide an image stream.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Plugin {source} not found.");
            }
        }
    }
}

