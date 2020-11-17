using camicmosserver.listeners;
using camicmosserver.listeners.repo;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace camicmosserver.listeners
{
    public class ListenersFactory
    {
        public const string LISTENER_TYPE_SERIAL = "serial";

        private readonly IListenersSource _source;
        private readonly List<IListener> _listeners;


        public ListenersFactory(IListenersSource source = null)
        {
            if (source!=null)
            {
                _source = source;
            }
            else
            {
                if (ListenerConfigReader.IsConfigFileExists())
                {
                    _source = new ListenerConfigReader();
                }
                else
                {
                    _source=  new DefaultListeners();
                }
            }
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
            if (name== LISTENER_TYPE_SERIAL || name=="device" || name=="camicmo")
            {
                return new CamicmoDevice();
            }
            return null;
        }

        private void LoadListeners()
        {
            foreach (var li in _source.Listeners)
            {
                try
                {
                    var l = LoadInstanceByName(li.Name);
                    if (l != null)
                    {
                        l.Init(li.Config);
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
