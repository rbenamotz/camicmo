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
        private int serialSpeed = 9600;
        public SerialPortHandler(dynamic config)
        {
            if (config==null)
            {
                return;
            }
            ApplyColor(_micOnColor, config["micOnColor"]);
            ApplyColor(_micOffColor, config["micOffColor"]);
            ApplyColor(_camOnColor, config["camOnColor"]);
            ApplyColor(_camOffColor, config["camOffColor"]);
            if (config.speed != null) { serialSpeed = (int)config.speed; }
        }

        internal void OnStateChanged(State state)
        {
            if (port == null || !port.IsOpen)
            {
                _pendingState = state;
                return;
            }
            if (state.IsSomethingOn)
            {
                for (int i=0; i<3; i++)
                {
                    SendToDevice(state);
                    Thread.Sleep(250);
                    SendToDevice(State.Empty);
                    Thread.Sleep(250);
                }
            }
            SendToDevice(state);
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

        public void FindSerialPort(int attempt = 1)
        {
            if (port!=null && port.IsOpen)
            {
                return;
            }
            Console.WriteLine("Looking for device, attempt " + attempt);
            foreach (var portName in SerialPort.GetPortNames())
            {
                if (CheckPort(portName))
                {
                    SendToDevice(_pendingState == null?_lastSentState :   _pendingState);
                    Console.WriteLine("Using port " + portName);
                    return;
                }
            }
            if (attempt<5)
            {
                Thread.Sleep(100);
                FindSerialPort(attempt + 1);
            }

        }


        private bool CheckPort(string portName)
        {
            Console.WriteLine("Chekcing port " + portName);
            try
            {
                port = new SerialPort(portName, serialSpeed, Parity.None, 8, StopBits.One)
                {
                    ReadTimeout = 500
                };
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

                Console.WriteLine("Sending " + b[0]);
                port.Write(b, 0, 7);
                Console.WriteLine("Reading...");
                port.Read(b, 0, 1);
                Console.WriteLine("Result: " + b[0]);
                if (b[0] == RESULT_OK)
                {
                    Console.WriteLine("Result ok");
                    return true;
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (InvalidOperationException)
            {
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Time out");
            }
            if (port.IsOpen) { port.Close(); }            
            port = null;
            return false;
        }

        private void SendToDevice(State state, int attempt = 1)
        {
            if (state ==null)
            {
                return;
            }
            try
            {
                Console.WriteLine("Sending colors to device, attempt " + attempt);
                byte[] buffer = new byte[7];
                buffer[0] = CMD_TYPE_SET_LED_COLORS;
                Array.Copy(state.IsCapbilityOn(State.MIC) ? _micOnColor : _micOffColor, 0, buffer, 1, 3);
                Array.Copy(state.IsCapbilityOn(State.WEBCAM) ? _camOnColor : _camOffColor, 0, buffer, 4, 3);
                port.Write(buffer, 0, 7);
                Console.Write("-->");
                for (int g=0; g<7; g++)
                {
                    if (g>0) { Console.Write(","); }
                    Console.Write(buffer[g]);
                }
                Console.Write("<--");
                port.Read(buffer, 0, 1);
                Console.Write(buffer[0]);
                Console.WriteLine("");
                if (buffer[0] != RESULT_OK && attempt<5)
                {
                    Console.WriteLine("Recieved " + buffer[0] + " from device. Retrying");
                    SendToDevice(state, attempt + 1);
                }
                _lastSentState = state;
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (attempt < 5)
                {
                    Console.WriteLine("Trying again");
                    SendToDevice(state, attempt + 1);
                }
            }
        }
    }
}
