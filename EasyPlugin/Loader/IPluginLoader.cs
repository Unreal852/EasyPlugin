using System.Reflection;
using System.Threading.Tasks;
using EasySharp.Safes;

namespace EasyPlugin.Loader
{
    public interface IPluginLoader<TPlugin>
    {
        TPlugin Load(Assembly assembly);

        Task<TPlugin> LoadAsync(Assembly assembly);

        bool TryLoadPlugin(Assembly assembly, out TPlugin plugin)
        {
            SafeResult<TPlugin> result = Safe.Try(() => Load(assembly));
            plugin = result.Result;
            return result.Success;
        }
    }
}