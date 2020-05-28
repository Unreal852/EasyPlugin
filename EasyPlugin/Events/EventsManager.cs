using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EasyPlugin.Events.Attributes;
using EasyPlugin.Plugin;
using EasySharp.Exceptions;
using EasySharp.Extensions;
using EasySharp.Reflection;

namespace EasyPlugin.Events
{
    public class EventsManager : IEventsManager
    {
        public EventsManager()
        {
        }

        private Dictionary<Type, List<EventHandlerInfo>> EventsHandlers { get; } = new Dictionary<Type, List<EventHandlerInfo>>();

        public void RegisterEvents(IPlugin plugin, object instance)
        {
            Throw.IfNull(instance, nameof(instance), "Instance should not be null.");
            RegisterEvents(plugin, instance.GetType());
        }

        public void RegisterEvents(IPlugin plugin, Type type, object instance = null)
        {
            foreach (MethodInfo methodInfo in type.GetAllMethods())
                RegisterEvent(plugin, methodInfo, instance);
            SortHandlers(type);
        }

        public void RegisterEvent(IPlugin plugin, MethodInfo method, object instance = null)
        {
            PluginEventHandler listener = method.GetCustomAttribute<PluginEventHandler>();
            if (listener == null || method.GetParameters().Length != 1)
                return;
            Type eventType = method.GetParameters()[0].ParameterType;
            if (!EventsHandlers.ContainsKey(eventType))
                EventsHandlers.Add(eventType, new List<EventHandlerInfo>());
            EventsHandlers[eventType].Add(new EventHandlerInfo(plugin, listener, method, instance));
        }

        public void UnregisterEvents(IPlugin plugin)
        {
            foreach (List<EventHandlerInfo> events in EventsHandlers.Values)
                events.RemoveAll(handler => handler.Plugin == plugin);
        }

        public void CallEvent(EventArgs @event)
        {
            Type eventType = @event.GetType();
            if (!EventsHandlers.ContainsKey(eventType))
                return;
            List<EventHandlerInfo> handlers = EventsHandlers[eventType];
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                EventHandlerInfo eventInfo = handlers[i];
                if (eventInfo.Handler.IgnoreCancelled && @event is CancelEventArgs cancelArgs && cancelArgs.Cancel)
                    continue;
                eventInfo.Action(@event);
                if (eventInfo.Handler.RemoveWhenUsed)
                    handlers.RemoveAt(i);
            }

            /*
            for (int i = 0; i < handlers.Length; i++)
            {
                EventHandlerInfo handlerInfo = handlers[i];
                if (handlerInfo.Handler.IgnoreCancelled && @event is CancelEventArgs cancelArgs && cancelArgs.Cancel)
                    continue;
                handlerInfo.Action(@event);
                if (handlerInfo.Handler.RemoveWhenUsed)
                    EventsHandlers[eventType].Remove(handlerInfo);
            } */
        }

        /// <summary>
        /// Sort handlers by their priority
        /// </summary>
        private void SortHandlers(Type type)
        {
            EventsHandlers[type].OrderByDescending(handler => (int) handler.Handler.Priority);
        }
    }
}