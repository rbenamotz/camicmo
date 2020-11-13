using System;
using System.IO.Ports;
using System.Management;
using System.Threading;

namespace camicmosserver.listeners
{
    class CamicmoDevice : IListener
    {
        private static Mutex mut = new Mutex();
        private  ManagementEventWatcher _watcher;
        private SerialPortHandler _serialPortHandler = null;

        public CamicmoDevice()
        {
        }

        private void CheckForNewPorts(EventArrivedEventArgs eventArgs)
        {
            Console.WriteLine("CheckForNewPorts");
            _serialPortHandler.FindSerialPort();
            
        }

        public void Init(dynamic config)
        {

            _serialPortHandler = new SerialPortHandler(config);
            _serialPortHandler.FindSerialPort();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent");
            _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += (sender, eventArgs) => CheckForNewPorts(eventArgs);
            _watcher.Start();
        }

        public void OnStateChanged(State state)
        {
            _serialPortHandler.OnStateChanged(state);
        }

    }
}
