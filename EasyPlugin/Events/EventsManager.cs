using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
            Validate.NotNull(instance, nameof(instance), "Instance should not be null.");
            RegisterEvents(plugin, instance.GetType());
        }

        public void RegisterEvents(IPlugin plugin, Type type, object instance = null)
        {
            foreach (MethodInfo methodInfo in type.GetAllMethods())
                RegisterEvent(plugin, methodInfo, instance);
        }

        public void RegisterEvent(IPlugin plugin, MethodInfo method, object instance = null)
        {
            EventListener listener = method.GetCustomAttribute<EventListener>();
            if (listener == null || method.GetParameters().Length != 1)
                return;
            Type eventType = method.GetParameters()[0].ParameterType;
            if(!EventsHandlers.ContainsKey(eventType))
                EventsHandlers.Add(eventType, new List<EventHandlerInfo>());
            EventsHandlers[eventType].Add(new EventHandlerInfo(plugin, listener, method, instance));
            SortHandlers(eventType);
        }

        public void UnregisterEvents(IPlugin plugin)
        {
            foreach (List<EventHandlerInfo> events in EventsHandlers.Values)
                events.RemoveAll(handler => handler.Plugin == plugin);
        }

        public void CallEvent(System.EventArgs @event)
        {
            Type eventType = @event.GetType();
            if (!EventsHandlers.ContainsKey(eventType))
                return;
            EventHandlerInfo[] handlers = EventsHandlers[eventType].ToArray();
            for (int i = 0; i < handlers.Length; i++)
            {
                EventHandlerInfo handlerInfo = handlers[i];
                if (handlerInfo.Listener.IgnoreCancelled && @event is CancelEventArgs cancelArgs && cancelArgs.Cancel)
                    continue;
                handlerInfo.Action(@event);
                if (handlerInfo.Listener.RemoveWhenUsed)
                    EventsHandlers[eventType].Remove(handlerInfo);
            }
        }

        /// <summary>
        /// Sort handlers by their priority
        /// </summary>
        private void SortHandlers(Type type)
        {
            EventsHandlers[type].OrderBy(handler => (int) handler.Listener.Priority);
        }
    }
}