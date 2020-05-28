using System;
using EasyPlugin.Plugin;

namespace EasyPlugin.Events.Args
{
    public class PluginLoadedEventArgs : EventArgs
    {
        public PluginLoadedEventArgs(IPlugin plugin)
        {
            Plugin = plugin;
        }
        
        /// <summary>
        /// The plugin that has been loaded
        /// </summary>
        public IPlugin Plugin { get; }
    }
}