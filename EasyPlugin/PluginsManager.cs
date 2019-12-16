using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using EasyPlugin.EventArgs;
using EasyPlugin.Events;
using EasyPlugin.Plugin;
using EasyPlugin.Plugin.Loader;

namespace EasyPlugin
{
    
    public class PluginsManager : IPluginsManager
    {
        public PluginsManager()
        {

        }

        public ReadOnlyCollection<IPlugin> Plugins => ListPlugins.AsReadOnly();
        
        public IEventsManager EventsManager { get; } = new EventsManager();

        private List<IPlugin> ListPlugins { get; } = new List<IPlugin>();

        public bool LoadPlugin<TPlugin>(FileInfo fileInfo, IPluginLoader<TPlugin> pluginLoader = null) where TPlugin : class, IPlugin
        {
            pluginLoader ??= new PluginLoader<TPlugin>(this);
            TPlugin plugin = pluginLoader.Load(fileInfo);
            if (plugin == null)
                return false;
            ListPlugins.Add(plugin);
            PluginLoadedEventArgs loadedEventArgs = new PluginLoadedEventArgs(plugin);
            EventsManager.CallEvent(loadedEventArgs);
            return true;
        }

        public Task<bool> LoadPluginAsync<TPlugin>(FileInfo fileInfo, IPluginLoader<TPlugin> pluginLoader = null) where TPlugin : class, IPlugin
        {
           throw new NotImplementedException();
        }

        public void UnloadPlugin(IPlugin plugin)
        {
            if (!ListPlugins.Contains(plugin))
                return;
            EventsManager.UnregisterEvents(plugin);
            ListPlugins.Remove(plugin);
            PluginUnloadedEventArgs unloadedEventArgs = new PluginUnloadedEventArgs(plugin);
            EventsManager.CallEvent(unloadedEventArgs);
        }
    }
}