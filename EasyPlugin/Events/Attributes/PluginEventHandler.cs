using System;

namespace EasyPlugin.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class PluginEventHandler : Attribute
    {
        public PluginEventHandler(EPriority eventPriority = EPriority.Normal, bool ignoreCancelled = false, bool removeWhenUsed = false)
        {
            Priority = eventPriority;
            IgnoreCancelled = ignoreCancelled;
            RemoveWhenUsed = removeWhenUsed;
        }

        /// <summary>
        /// Event Priority.
        /// </summary>
        public EPriority Priority { get; }

        /// <summary>
        /// If set to true, this event will not be called if the event has been cancelled previously.
        /// </summary>
        public bool IgnoreCancelled { get; }

        /// <summary>
        /// If set to true, this event will be unregistered after his first call.
        /// </summary>
        public bool RemoveWhenUsed { get; }
    }
}