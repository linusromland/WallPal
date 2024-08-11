using System.Reflection;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace WallPal
{
    public class PluginManager
    {
        private readonly string _pluginDirectory;
        private readonly List<IPlugin> _plugins;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PluginManager(string pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
            _plugins = [];
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            foreach (var file in Directory.GetFiles(_pluginDirectory, "*.dll"))
            {
                _logger.Info($"Loading plugin: {file}");
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (Type type in assembly.GetTypes())
                {
                    string pluginName = type.Name;
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        ConstructorInfo? constructor = type.GetConstructor([typeof(IApplicationServices)]);

                        if (constructor == null)
                        {
                            _logger.Error($"Type {pluginName} does not have a constructor that accepts IApplicationServices.");
                            continue;
                        }

                        if (_plugins.Any(p => p.Name == pluginName))
                        {
                            _logger.Warn($"Plugin {pluginName} is already loaded.");
                            continue;
                        }

                        ServiceCollection serviceCollection = new();
                        serviceCollection.AddSingleton<IApplicationServices>(new ApplicationServices(pluginName));
                        ServiceProvider? serviceProvider = serviceCollection.BuildServiceProvider();
                        IApplicationServices? appServices = serviceProvider.GetRequiredService<IApplicationServices>();


                        IPlugin plugin = (IPlugin)constructor.Invoke([appServices]);

                        _logger.Info($"Plugin {pluginName} loaded.");
                        _plugins.Add(plugin);
                    }
                }
            }
        }

        public IPlugin? GetPlugin(string pluginName)
        {
            return _plugins.FirstOrDefault(p => p.Name == pluginName);
        }
    }
}
