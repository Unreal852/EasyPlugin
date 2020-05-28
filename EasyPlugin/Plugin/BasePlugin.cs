namespace EasyPlugin.Plugin
{
    public abstract class BasePlugin : IPlugin
    {
        protected BasePlugin()
        {
        }

        public IPluginsManager PluginsManager { get; internal set; }

        public abstract void OnEnable();

        public abstract void OnDisable();
    }
}