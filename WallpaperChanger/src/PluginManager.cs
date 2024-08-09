using System.Reflection;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WallPal
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
                    string pluginName = type.Name;
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        ConstructorInfo? constructor = type.GetConstructor([typeof(IApplicationServices)]);

                        if (constructor == null)
                        {
                            Console.WriteLine($"Type {pluginName} does not have a constructor that accepts IApplicationServices.");
                            continue;
                        }

                        if (_plugins.Any(p => p.Name == pluginName))
                        {
                            Console.WriteLine($"Plugin {pluginName} is already loaded.");
                            continue;
                        }

                        ServiceCollection serviceCollection = new ServiceCollection();
                        serviceCollection.AddSingleton<IApplicationServices>(new ApplicationServices(pluginName));

                        // Build service provider
                        ServiceProvider? serviceProvider = serviceCollection.BuildServiceProvider();
                        IApplicationServices? appServices = serviceProvider.GetRequiredService<IApplicationServices>();


                        IPlugin plugin = (IPlugin)constructor.Invoke([appServices]);

                        Console.WriteLine($"Plugin {pluginName} loaded.");
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
