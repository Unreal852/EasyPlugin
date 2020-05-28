using System;
using System.Reflection;
using EasyPlugin.Events.Attributes;
using EasyPlugin.Plugin;
using EasySharp.Reflection;

namespace EasyPlugin.Events
{
    public class EventHandlerInfo
    {
        public EventHandlerInfo(IPlugin plugin, PluginEventHandler eventHandler, MethodInfo methodInfo, object instance = null)
        {
            Plugin = plugin;
            Handler = eventHandler;
            Instance = instance;
            Action = ExpressionHelper.CreateDelegate<Action<EventArgs>>(methodInfo, instance);
        }

        /// <summary>
        /// Plugin
        /// </summary>
        public IPlugin Plugin { get; }

        /// <summary>
        /// Event Handler
        /// </summary>
        public PluginEventHandler Handler { get; }

        /// <summary>
        /// Event Handler Method
        /// </summary>
        public Action<EventArgs> Action { get; }

        /// <summary>
        /// Method's Instance ( may be null for static members ).
        /// </summary>
        public object Instance { get; }
    }
}