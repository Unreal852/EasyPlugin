using System.IO;
using System.Threading.Tasks;

namespace EasyPlugin.Plugin.Loader
{
    public interface IPluginLoader<TPlugin> where TPlugin : class, IPlugin
    {
        /// <summary>
        /// Tries to load the specified file as a plugin.
        /// </summary>
        /// <param name="file">Plugin File</param>
        /// <returns><see cref="TPlugin"/> if the plugin has been loaded, null otherwise</returns>
        TPlugin Load(FileInfo file);

        /// <summary>
        /// Tries to load the specified file as a plugin asynchronously.
        /// </summary>
        /// <param name="file">Plugin File</param>
        /// <returns><see cref="TPlugin"/> if the plugin has been loaded, null otherwise</returns>
        Task<TPlugin> LoadAsync(FileInfo file);
    }
}