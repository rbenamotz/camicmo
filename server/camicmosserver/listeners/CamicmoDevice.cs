using System;
using System.IO.Ports;
using System.Threading;

namespace camicmosserver.listeners
{
    class CamicmoDevice : IListener
    {
        private static Mutex mut = new Mutex();
        private SerialPortHandler _serialPortHandler;

        public CamicmoDevice()
        {
        }

        public void Init(dynamic config)
        {
            _serialPortHandler = new SerialPortHandler(config);
            Thread t = new Thread(new ThreadStart(_serialPortHandler.Run));
            t.Start();
        }

        public void OnStateChanged(State state)
        {
            _serialPortHandler.OnStateChanged(state);
        }

    }
}
