using System;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WallpaperChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            WallpaperChanger wallpaperChanger = new WallpaperChanger();

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IApplicationServices, ApplicationServices>();

            // Build service provider
            ServiceProvider? serviceProvider = serviceCollection.BuildServiceProvider();
            IApplicationServices? appServices = serviceProvider.GetRequiredService<IApplicationServices>();

            string pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
            PluginManager pluginManager = new PluginManager(pluginDirectory, appServices);
            IPlugin? plugin = pluginManager.GetPlugins().FirstOrDefault();

            if (plugin != null)
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
            else
            {
                Console.WriteLine("No plugins found.");
            }
        }
    }
}

