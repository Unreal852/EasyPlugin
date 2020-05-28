using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using static System.Runtime.Loader.AssemblyLoadContext;

namespace EasyPlugin.Loader.Assemblies
{
    public class AssemblyLoaderBuilder
    {
        private readonly List<string>        _privateDependencies = new List<string>();
        private readonly List<string>        _sharedDependencies  = new List<string>();
        private          AssemblyLoadContext _defaultLoadContext  = GetLoadContext(Assembly.GetExecutingAssembly()) ?? Default;
        private          string              _mainAssemblyPath;

        public AssemblyLoaderBuilder()
        {
        }

        public bool                  IsCollectible       { get; set; } = true;
        public bool                  LoadInMemory        { get; set; } = false;
        public IReadOnlyList<string> PrivateDependencies => _privateDependencies.AsReadOnly();
        public IReadOnlyList<string> SharedDependencies  => _sharedDependencies.AsReadOnly();

        public string MainAssemblyPath
        {
            get => _mainAssemblyPath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Argument must not be null or empty", nameof(value));
                if (Path.IsPathRooted(value))
                    throw new ArgumentException("Argument must be a valid path", nameof(value));
                _mainAssemblyPath = value;
            }
        }

        public AssemblyLoadContext DefaultLoadContext
        {
            get => _defaultLoadContext;
            set => _defaultLoadContext = value ?? throw new ArgumentNullException(nameof(value));
        }

        public AssemblyLoaderBuilder SetMainAssembly(string path)
        {
            MainAssemblyPath = path;
            return this;
        }

        public AssemblyLoaderBuilder AddPrivateDependency(AssemblyName assemblyName)
        {
            ValidateAssemblyName(assemblyName);
            if (!_privateDependencies.Contains(assemblyName.Name))
                _privateDependencies.Add(assemblyName.Name);
            return this;
        }

        public AssemblyLoaderBuilder AddSharedDependency(AssemblyName assemblyName, bool shareDependencies = true)
        {
            ValidateAssemblyName(assemblyName);
            if (!_sharedDependencies.Contains(assemblyName.Name))
                _sharedDependencies.Add(assemblyName.Name);
            if (!shareDependencies)
                return this;
            Assembly assembly = DefaultLoadContext.LoadFromAssemblyName(assemblyName);
            foreach (AssemblyName depName in assembly.GetReferencedAssemblies())
                AddSharedDependency(depName);
            return this;
        }

        public AssemblyLoaderBuilder SetDefaultLoadContext(AssemblyLoadContext defaultLoadContext)
        {
            DefaultLoadContext = defaultLoadContext;
            return this;
        }

        public AssemblyLoaderBuilder SetCollectible(bool isCollectible)
        {
            IsCollectible = isCollectible;
            return this;
        }

        public AssemblyLoaderBuilder SetLoadInMemory(bool loadInMemory)
        {
            LoadInMemory = loadInMemory;
            return this;
        }

        public ModernAssemblyLoader Build()
        {
            if (string.IsNullOrWhiteSpace(MainAssemblyPath))
                throw new ArgumentException("Argument must not be null or empty", nameof(MainAssemblyPath));
            if (Path.IsPathRooted(MainAssemblyPath))
                throw new ArgumentException("Argument must be a valid path", nameof(MainAssemblyPath));
            return new ModernAssemblyLoader(MainAssemblyPath, DefaultLoadContext, PrivateDependencies, SharedDependencies, IsCollectible, LoadInMemory);
        }

        private void ValidateAssemblyName(AssemblyName assemblyName)
        {
            if (assemblyName?.Name == null)
                throw new ArgumentNullException(nameof(assemblyName));
        }
    }
}