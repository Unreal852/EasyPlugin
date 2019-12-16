using System;
using System.Reflection;
using EasyPlugin.Plugin;
using EasySharp.Reflection;

namespace EasyPlugin.Events
{
    public class EventHandlerInfo
    {
        public EventHandlerInfo(IPlugin plugin, EventListener eventListener, MethodInfo methodInfo, object instance = null)
        {
            Plugin = plugin;
            Listener = eventListener;
            Instance = instance;
            Action = methodInfo.ToDelegate<Action<System.EventArgs>>(instance);
        }
        
        /// <summary>
        /// Plugin
        /// </summary>
        public IPlugin Plugin { get; }
        
        /// <summary>
        /// Event Listener
        /// </summary>
        public EventListener Listener { get; }
        
        /// <summary>
        /// Event Handler Method
        /// </summary>
        public Action<System.EventArgs> Action { get; }
    
        /// <summary>
        /// Method's Instance ( may be null for static members ).
        /// </summary>
        public object Instance { get; }
    }
    
}