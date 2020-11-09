using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver
{
    class SerialHandler
    {
        private const int CMD_SET_COLORS = 0x02;

        // Create the serial port with basic settings
        private SerialPort port = new SerialPort("COM10",115200, Parity.None, 8, StopBits.One);
        private Random r = new Random();


        public SerialHandler()
        {
            var ff = SerialPort.GetPortNames();

            port.Open();
        }

        public void SetColors()
        {

            if (!port.IsOpen)
            {
                return;
            }
        }

        internal void Update(Preferences prefs,  State state)
        {
            if (!port.IsOpen)
            {
                throw new Exception("Port not open");
            }
            byte[] b = new byte[7];
            b[0] = CMD_SET_COLORS;
            b[1] = state.IsCamOn ? prefs.CamOnColor.red : prefs.CamOffColor.red;
            b[2] = state.IsCamOn ? prefs.CamOnColor.green : prefs.CamOffColor.green;
            b[3] = state.IsCamOn ? prefs.CamOnColor.blue : prefs.CamOffColor.blue;
            b[4] = state.IsMicOn ? prefs.MicOnColor.red : prefs.MicOffColor.red;
            b[5] = state.IsMicOn ? prefs.MicOnColor.green : prefs.MicOffColor.green;
            b[6] = state.IsMicOn ? prefs.MicOnColor.blue : prefs.MicOffColor.blue;
            port.Write(b, 0, 7);
            Console.Write(b[0]);
            port.Read(b, 0, 1);
            Console.Write("-->");
            Console.WriteLine(b[0]);
        }
    }
}
