using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using EasyPlugin.Plugin;

namespace EasyPlugin.EventArgs
{
    public class PluginLoadedEventArgs : System.EventArgs
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