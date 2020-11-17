using camicmosserver.listeners.repo;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace camicmosserver.listeners.repo
{
    public class ListenerConfigReader : IListenersSource
    {
        private readonly List<ListenerInfo> _listeners;

        public IEnumerable<ListenerInfo> Listeners => _listeners;

        public ListenerConfigReader()
        {
            _listeners = new List<ListenerInfo>();
            var configFile = ConfigFileName();
            if (!File.Exists(configFile))
            {
                return;
            }
            using (StreamReader r = new StreamReader(configFile))
            {
                string s = r.ReadToEnd();
                var all = JArray.Parse(s);
                foreach (dynamic a in all)
                {
                    _listeners.Add(new ListenerInfo((string)a.type, a.config));
                }
            }

        }
        private static string ConfigFileName()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = assemblyPath + "\\" + "listeners.json0";
            return configFile;

        }
        internal static bool IsConfigFileExists()
        {
            return (File.Exists(ConfigFileName()));
        }
    }
}
