using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace camicmosserver
{
    class Program
    {
        private Preferences _prefs;
        private State _state;
        private SerialHandler _serialHandler;
        private RegListener _regListener;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var p = new Program();
            p.Run();
        }
        private Program()
        {
            _prefs = new Preferences();
            _state = new State(false,false);
            _serialHandler = new SerialHandler();
            _regListener = new RegListener();
        }

        private void Run()
        {
            _serialHandler.Update(_prefs, _state);
            while (true)
            {
                _state.IsCamOn = _regListener.CheckAccessFor("webcam");
                _state.IsMicOn = _regListener.CheckAccessFor("microphone");
                if (_state.IsDirty)
                {
                    _serialHandler.Update(_prefs, _state);
                    _state.IsDirty = false;

                }
                Thread.Sleep(100);
            }

        }
    }
}
