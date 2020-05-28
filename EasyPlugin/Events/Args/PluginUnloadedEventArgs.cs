using System;
using EasyPlugin.Plugin;

namespace EasyPlugin.Events.Args
{
    public class PluginUnloadedEventArgs : EventArgs
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