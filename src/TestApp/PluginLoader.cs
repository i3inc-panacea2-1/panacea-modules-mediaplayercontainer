using Panacea.Core;
using Panacea.Modularity;
using Panacea.Modularity.Media;
using Panacea.Modules.MediaPlayerContainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestApp
{
    class PluginLoader : IPluginLoader
    {
        public PluginLoader()
        {

        }

        public void LoadPlugin(IPlugin plugin)
        {
            _plugins.Add(plugin);
            PluginLoaded?.Invoke(this, plugin);
        }

        public void UnloadPlugin(IPlugin plugin)
        {
            _plugins.Remove(plugin);
            PluginUnloaded?.Invoke(this, plugin);
        }

        private readonly List<IPlugin> _plugins = new List<IPlugin>();
        public IReadOnlyDictionary<string, IPlugin> LoadedPlugins => throw new NotImplementedException();

        public event EventHandler<IPlugin> PluginLoaded;
        public event EventHandler<IPlugin> PluginUnloaded;

        public string GetPluginDirectory(string pluginName)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Plugins", pluginName);
        }

        public IEnumerable<T> GetPlugins<T>() where T : IPlugin
        {
            return _plugins.Where(p => typeof(T).IsAssignableFrom(p.GetType())).Cast<T>();
        }
    }
}
