using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using EasyPlugin.Events;
using EasyPlugin.Loader;
using EasyPlugin.Plugin;

namespace EasyPlugin
{
    public interface IPluginsManager
    {
        /// <summary>
        /// Events Manager
        /// </summary>
        IEventsManager EventsManager { get; }

        /// <summary>
        /// Loaded Plugins
        /// </summary>
        ReadOnlyCollection<IPlugin> Plugins { get; }

        /// <summary>
        /// Tries to load the specified file.
        /// </summary>
        /// <param name="fileInfo">The file to load</param>
        /// <param name="pluginLoader">The plugin loader</param>
        /// <returns>true if the file has been successfully loaded, false otherwise</returns>
        bool LoadPlugin<TPlugin>(FileInfo fileInfo, IPluginLoader<TPlugin> pluginLoader = null) where TPlugin : class, IPlugin;

        /// <summary>
        /// Tries to load the specified file asynchronously.
        /// </summary>
        /// <param name="fileInfo">The file to load</param>
        /// <param name="pluginLoader">The plugin loader</param>
        /// <returns>true if the file has been successfully loaded, false otherwise</returns>
        Task<bool> LoadPluginAsync<TPlugin>(FileInfo fileInfo, IPluginLoader<TPlugin> pluginLoader = null) where TPlugin : class, IPlugin;

        /// <summary>
        /// Unload the specified plugin.
        /// </summary>
        /// <param name="plugin">The plugin to unload</param>
        void UnloadPlugin(IPlugin plugin);
    }
}