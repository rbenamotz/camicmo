using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver.listeners.repo
{
    public struct ListenerInfo
    {
        public ListenerInfo(string name, dynamic config)
        {
            this.Name = name;
            this.Config = config;
        }
        public String Name { get; private set; }
        public dynamic Config { get; private set; }
    }
    public interface IListenersSource
    {
        IEnumerable<ListenerInfo> Listeners { get; }
    }
}
