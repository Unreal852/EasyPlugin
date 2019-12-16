using System;
using EasyPlugin.Plugin;

namespace EasyPlugin.EventArgs
{
    public class PluginUnloadedEventArgs : System.EventArgs
    {
        public PluginUnloadedEventArgs(IPlugin plugin)
        {
            Plugin = plugin;
        }
        
        /// <summary>
        /// The plugin that has been unloaded
        /// </summary>
        public IPlugin Plugin { get; }
    }
}