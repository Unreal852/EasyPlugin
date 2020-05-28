using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace EasyPlugin.Loader.Assemblies
{
    public class ModernAssemblyLoader : AssemblyLoadContext, IAssemblyLoader
    {
        public ModernAssemblyLoader(string mainAssemblyPath, AssemblyLoadContext defaultLoadContext = null,
                                    IReadOnlyList<string> privateAssemblies = null, IReadOnlyList<string> sharedAssemblies = null,
                                    bool isCollectible = true, bool loadInMemory = false) : base(Path.GetFileNameWithoutExtension(mainAssemblyPath), isCollectible)
        {
            MainAssemblyPath = mainAssemblyPath ?? throw new ArgumentNullException(nameof(mainAssemblyPath));
            PrivateAssemblies = privateAssemblies ?? new List<string>().AsReadOnly();
            SharedAssemblies = sharedAssemblies ?? new List<string>().AsReadOnly();
            DependencyResolver = new AssemblyDependencyResolver(mainAssemblyPath);
            DefaultLoadContext = defaultLoadContext ?? GetLoadContext(Assembly.GetExecutingAssembly()) ?? Default;
        }

        public  string                     MainAssemblyPath   { get; }
        private IReadOnlyList<string>      PrivateAssemblies  { get; }
        private IReadOnlyList<string>      SharedAssemblies   { get; }
        private AssemblyDependencyResolver DependencyResolver { get; }
        private AssemblyLoadContext        DefaultLoadContext { get; }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (SharedAssemblies.Contains(assemblyName.Name))
                return DefaultLoadContext.LoadFromAssemblyName(assemblyName);
            string assemblyPath = DependencyResolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = DependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath != null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
        }

        public Assembly LoadFromFile(string filePath)
        {
            return Load(AssemblyName.GetAssemblyName(filePath));
        }

        public Assembly LoadFromName(AssemblyName assemblyName)
        {
            return Load(assemblyName);
        }
    }
}