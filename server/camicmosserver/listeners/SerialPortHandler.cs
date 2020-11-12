using System;
using System.IO.Ports;
using System.Threading;

namespace camicmosserver.listeners
{
    class SerialPortHandler
    {
        private SerialPort port = null;
        private State _pendingState = null;
        private State _lastSentState = null;
        private const byte CMD_TYPE_HANDSHAKE = 0xC8;
        private const byte CMD_TYPE_SET_LED_COLORS = 0xC9;
        private const byte RESULT_OK = 0x64;


        private byte[] _micOnColor = { 255, 0, 0 };
        private byte[] _micOffColor = { 0, 10, 0 };
        private byte[] _camOnColor = { 255, 0, 0 };
        private byte[] _camOffColor = { 0, 10, 0 };

        public SerialPortHandler(dynamic config)
        {
            ApplyColor(_micOnColor, config["micOnColor"]);
            ApplyColor(_micOffColor, config["micOffColor"]);
            ApplyColor(_camOnColor, config["camOnColor"]);
            ApplyColor(_camOffColor, config["camOffColor"]);
        }

        internal void OnStateChanged(State state)
        {
            _pendingState = state;
        }

        private void ApplyColor(byte[] b, dynamic dynamic)
        {
            if (dynamic == null)
            {
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                b[i] = (byte)dynamic[i];
            }
        }


        public void Run()
        {
            var lastConnctionAttempt = DateTime.MinValue;
            while (true)
            {
                if (port != null && port.IsOpen)
                {
                    SendPendingState();
                    Thread.Sleep(100);
                    continue;
                }
                var ts = DateTime.Now - lastConnctionAttempt;
                if (ts.TotalSeconds<=2)
                {
                    continue;
                }
                Console.WriteLine("Need to connect to serial");
                foreach (var portName in SerialPort.GetPortNames())
                {
                    Console.WriteLine(portName);
                    if (CheckPort(portName))
                    {
                        if (_pendingState == null) { SendToDevice(_lastSentState); };
                        break;
                    }
                }
                lastConnctionAttempt = DateTime.Now;
            }

        }
        private void SendPendingState()
        {
            if (_pendingState == null)
            {
                return;
            }
            lock (_pendingState)
            {
                SendToDevice(_pendingState);
                _pendingState = null;
            }
        }

        private bool CheckPort(string portName)
        {
            try
            {
                port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                port.ReadTimeout = 100;
                port.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                port = null;
                return false;
            }
            byte[] b = new byte[7];
            for (int i = 0; i < 7; i++)
            {
                b[i] = 0x00;
            }
            b[0] = CMD_TYPE_HANDSHAKE;
            try
            {
                Console.Write(b[0]);
                port.Write(b, 0, 7);
                port.Read(b, 0, 1);
                Console.Write("-->");
                Console.WriteLine(b[0]);
                if (b[0] == RESULT_OK)
                {
                    return true;
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (System.TimeoutException)
            {
            }
            port.Close();
            port = null;
            return false;
        }

        private void SendToDevice(State state)
        {
            if (state == null)
            {
                return;
            }
            Console.Write(CMD_TYPE_SET_LED_COLORS);

            byte[] buffer = new byte[] { CMD_TYPE_SET_LED_COLORS };
            port.Write(buffer, 0, 1);
            port.Write(state.IsCamOn ? _camOnColor : _camOffColor, 0, 3);
            port.Write(state.IsMicOn ? _micOnColor : _micOffColor, 0, 3);
            port.Read(buffer, 0, 1);
            Console.Write("-->");
            Console.WriteLine(buffer[0]);
            _lastSentState = state;
        }
    }
}
