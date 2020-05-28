using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EasyPlugin.Plugin;

namespace EasyPlugin.Loader
{
    public class PluginLoader<TPlugin> : IPluginLoader<TPlugin> where TPlugin : class, IPlugin
    {
        public PluginLoader(IPluginsManager pluginsManager)
        {
            PluginsManager = pluginsManager;
        }

        public IPluginsManager PluginsManager { get; }

        public TPlugin Load(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            TPlugin pluginInstance = LoadMainType(assembly);
            if (pluginInstance == default)
                throw new Exception($"Failed to load '{typeof(TPlugin).Name}' in assembly '{assembly.GetName().Name}'. A plugin must be a class implementing the 'IPlugin' interface.");
            if (pluginInstance is BasePlugin basePlugin)
            {
                basePlugin.PluginsManager = PluginsManager;
            }

            pluginInstance.OnEnable();
            return pluginInstance;
        }

        public Task<TPlugin> LoadAsync(Assembly assembly)
        {
            throw new System.NotImplementedException();
        }

        protected virtual TPlugin LoadMainType(Assembly assembly)
        {
            Type pluginType = typeof(TPlugin);
            Type interfacePluginType = typeof(IPlugin);
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && !type.IsInterface && interfacePluginType.IsAssignableFrom(type) && pluginType.IsAssignableFrom(type))
                    return Activator.CreateInstance(type) as TPlugin;
            }

            return default;
        }
    }
}