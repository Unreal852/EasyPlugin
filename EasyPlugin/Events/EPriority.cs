namespace EasyPlugin.Events
{
    public enum EPriority
    {
        /// <summary>
        /// Lowest priority, this will be called first.
        /// </summary>
        Lowest,
        Low,
        Normal,
        High,
        Highest,

        /// <summary>
        /// MONITOR priority, this will be called last.
        /// </summary>
        Monitor,
    }
}