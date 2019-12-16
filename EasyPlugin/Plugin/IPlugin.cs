namespace EasyPlugin.Plugin
{
    /// <summary>
    /// Provide an interface to be implemented by plugins.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Plugins Manager.
        /// The owner manager of this plugin.
        /// </summary>
        public IPluginsManager PluginsManager { get; }

        /// <summary>
        /// The plugin data folder.
        /// This is a folder dedicated to this plugin.
        /// </summary>
        public string DataFolder { get; }

        /// <summary>
        /// Called when this plugin is being enabled.
        /// </summary>
        public void OnEnable();

        /// <summary>
        /// Called when this plugin is disabled.
        /// </summary>
        public void OnDisable();
    }
}