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

        public PluginManager(string pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
            _plugins = new List<IPlugin>();
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
                        Console.WriteLine($"Plugin found: {type.FullName}");
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        _plugins.Add(plugin);
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
