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
        // Create the serial port with basic settings
        private SerialPort port = new SerialPort("COM5",115200, Parity.None, 8, StopBits.One);

        public SerialHandler()
        {
            var ff = SerialPort.GetPortNames();

            port.Open();
        }

        public void SetColors(string s)
        {
            port.Write(s);
        }
    }
}
