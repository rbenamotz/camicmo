using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver.listeners
{
    public interface IListener
    {
        void Init(dynamic config);
        void OnStateChanged(State state);
    }
}
