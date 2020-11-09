using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver
{
    public struct Color
    {
        public Color(byte r, byte g, byte b)
        {
            this.red = r;
            this.green = g;
            this.blue = b;
        }
        public byte red;
        public byte green;
        public byte blue;
    }
    public class Preferences
    {

        public Preferences()
        {
            this.CamOnColor = new Color(100, 0, 0);
            this.CamOffColor = new Color(0, 10, 0);
            this.MicOnColor = new Color(100, 0, 0);
            this.MicOffColor = new Color(0, 10, 0);
        }

        public Color CamOnColor { get; private set; }
        public Color CamOffColor { get; private set; }
        public Color MicOnColor { get; private set; }
        public Color MicOffColor { get; private set; }
    }
}
