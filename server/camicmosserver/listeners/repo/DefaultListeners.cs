using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver.listeners.repo
{
    class DefaultListeners : IListenersSource
    {
        private readonly List<ListenerInfo> _listeners;
        public IEnumerable<ListenerInfo> Listeners => _listeners;

        public DefaultListeners()
        {
            _listeners = new List<ListenerInfo>
            {
                new ListenerInfo(ListenersFactory.LISTENER_TYPE_SERIAL, null)
            };
        }
    }
}
