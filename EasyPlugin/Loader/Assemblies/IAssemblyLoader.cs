using System.Reflection;
using EasySharp.Safes;

namespace EasyPlugin.Loader.Assemblies
{
    public interface IAssemblyLoader
    {
        Assembly LoadFromFile(string filePath);
        Assembly LoadFromName(AssemblyName assemblyName);

        bool TryLoadFromFile(string filePath, out Assembly assembly)
        {
            SafeResult<Assembly> result = Safe.Try(() => LoadFromFile(filePath));
            assembly = result.Result;
            return result.Success;
        }

        bool TryLoadFromName(AssemblyName assemblyName, out Assembly assembly)
        {
            SafeResult<Assembly> result = Safe.Try(() => LoadFromName(assemblyName));
            assembly = result.Result;
            return result.Success;
        }
    }
}