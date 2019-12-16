using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EasySharp.Exceptions;

namespace EasyPlugin.Plugin.Loader
{
    public class PluginLoader<TPlugin> : IPluginLoader<TPlugin> where TPlugin : class, IPlugin
    {
        public PluginLoader(IPluginsManager pluginsManager)
        {
            PluginsManager = pluginsManager;
        }
        
        /// <summary>
        /// The plugins manager owner of this plugin loader.
        /// </summary>
        public IPluginsManager PluginsManager { get; }
        
        public TPlugin Load(FileInfo file)
        {
            Validate.Require<FileNotFoundException>(file.Exists, nameof(file), "The specified file does not exists.");
            Assembly pluginAssembly = Assembly.LoadFile(file.FullName);
            TPlugin plugin = LoadPlugin(pluginAssembly);
            if (plugin is BasePlugin basePlugin)
            {
                basePlugin.PluginsManager = PluginsManager;
                basePlugin.DataFolder =
                    Path.Combine(Path.GetDirectoryName(file.FullName), $@"\{pluginAssembly.FullName}");
            }
            plugin.OnEnable();
            return plugin;
        }

        public Task<TPlugin> LoadAsync(FileInfo file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to load a plugin from the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly to load from</param>
        /// <returns><see cref="TPlugin"/></returns>
        protected TPlugin LoadPlugin(Assembly assembly)
        {
            Type mainType = FindPluginType(assembly);
            TPlugin pluginInstance = (TPlugin)Activator.CreateInstance(mainType);
            return pluginInstance;
        }

        /// <summary>
        /// Tries to find the plugin main type.
        /// </summary>
        /// <param name="assembly">Assembly to load from</param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown is no valid type is found.</exception>
        protected Type FindPluginType(Assembly assembly)
        {
            Type pluginType = typeof(TPlugin);
            Type interfacePluginType = typeof(IPlugin);
            foreach (Type type in assembly.GetTypes())
            {
                if (pluginType.IsAssignableFrom(type) && interfacePluginType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && !type.IsInterface)
                    return type;
            }
            throw new Exception("Could not find a valid plugin type. A plugin must be a class implementing the IPlugin interface.");
        }
        
        /*
        protected virtual bool LoadDependencies(FileInfo file, Type type)
        {
            DirectoryInfo directoryInfo = file.Directory;
            foreach (PluginDepend depend in type.GetCustomAttributes<PluginDepend>())
            {
                FileInfo dependFile = new FileInfo(Path.Combine(directoryInfo?.FullName, depend.FileName));
                if (!dependFile.Exists && depend.IsRequired)
                    return false;
                if (!PluginsManager.LoadPlugin(dependFile) && depend.IsRequired)
                    return false;
            }

            return true;
        } */
    }
}