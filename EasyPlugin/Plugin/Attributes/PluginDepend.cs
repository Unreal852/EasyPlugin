using System;
using System.ComponentModel.DataAnnotations;

namespace EasyPlugin.Plugin.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginDepend : ValidationAttribute
    {
        public PluginDepend(string fileName, bool required = true)
        {
            FileName = fileName;
            IsRequired = required;
        }
        
        /// <summary>
        /// Dependency file name.
        /// </summary>
        public string FileName { get; }
        
        /// <summary>
        /// If set to false, this is a optional dependency file.
        /// </summary>
        public bool IsRequired { get; }
    }
}