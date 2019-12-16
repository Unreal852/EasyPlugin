using System;
using System.Reflection;
using EasyPlugin.Plugin;

namespace EasyPlugin.Events
{
    public interface IEventsManager
    {
        void RegisterEvents(IPlugin plugin, object instance);

        void RegisterEvents(IPlugin plugin, Type type, object instance = null);

        void RegisterEvent(IPlugin plugin, MethodInfo method, object instance = null);

        void UnregisterEvents(IPlugin plugin);

        void CallEvent(System.EventArgs @event);
    }
}