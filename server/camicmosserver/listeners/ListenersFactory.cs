using camicmosserver.listeners;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace camicmosserver
{
    class ListenersFactory
    {
        private List<IListener> _listeners;

        public ListenersFactory()
        {
            _listeners = new List<IListener>();
            LoadListeners();
        }
        public IEnumerable<IListener> Listeners => _listeners;

        private IListener LoadInstanceByName(string name)
        {
            //TODO: Replace with proper reflection
            name = name.ToLower();
            if (name == "mqttbroker")
            {
                return new MqttBroker();
            }
            if (name=="serial" || name=="device" || name=="camicmo")
            {
                return new CamicmoDevice();
            }
            return null;
        }

        private void LoadListeners()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = assemblyPath + "\\" + "listeners.json";
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
                    try
                    {
                        var l = LoadInstanceByName((string) a.type);
                        if (l!=null)
                        {
                            l.Init(a.config);
                            _listeners.Add(l);
                        }
                    }
                    catch (RuntimeBinderException)
                    {
                        throw;
                        //TODO: Log
                    }
                }
            }
        }
    }
}
