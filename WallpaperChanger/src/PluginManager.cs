using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Interfaces;

namespace WallpaperChanger
{
    public class PluginManager
    {
        private readonly string _pluginDirectory;
        private readonly List<IPlugin> _plugins;

        private readonly IApplicationServices _appServices;

        public PluginManager(string pluginDirectory, IApplicationServices appServices)
        {
            _pluginDirectory = pluginDirectory;
            _plugins = new List<IPlugin>();
            _appServices = appServices;
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            foreach (var file in Directory.GetFiles(_pluginDirectory, "*.dll"))
            {
                Console.WriteLine($"Loading plugin: {file}");
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type, _appServices);
                        if (plugin.IsReady())
                        {
                            Console.WriteLine($"Plugin {plugin.Name} is ready.");
                            _plugins.Add(plugin);
                        }
                        else
                        {
                            Console.WriteLine($"Plugin {plugin.Name} is not ready.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Type {type.FullName} is not a plugin.");
                    }
                }
            }
        }

        public IEnumerable<IPlugin> GetPlugins()
        {
            return _plugins;
        }
    }
}
