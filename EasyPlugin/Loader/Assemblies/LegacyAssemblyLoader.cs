using System;
using System.IO;
using System.Reflection;

namespace EasyPlugin.Loader.Assemblies
{
    public class LegacyAssemblyLoader : IAssemblyLoader
    {
        public Assembly LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);
            return Assembly.LoadFile(filePath);
        }

        public Assembly LoadFromName(AssemblyName assemblyName)
        {
            if (assemblyName == null || string.IsNullOrWhiteSpace(assemblyName.Name))
                throw new ArgumentException("The specified AssemblyName is null or had no name", nameof(assemblyName));
            return Assembly.Load(assemblyName);
        }
    }
}