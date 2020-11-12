using camicmosserver.listeners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace camicmosserver
{
    class Program
    {
        private readonly IEnumerable<IListener> _listeners;
        private readonly State _state;
        private readonly RegListener _regListener;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "INSTALLER")
            {
                EventLog.WriteEntry("Application", "camicmo server called from installer");
                var e = System.Reflection.Assembly.GetEntryAssembly().Location;
                Process.Start(e);
                return;
            } 
            EventLog.WriteEntry("Application","camicmo server is loading");
            var p = new Program();
            p.Run();
        }
        private Program()
        {
            var l = new ListenersFactory();
            _listeners = l.Listeners;
            _state = new State(false,false);
            _regListener = new RegListener();
        }
        private void Run()
        {
            while (true)
            {
                _state.IsCamOn = _regListener.CheckAccessFor("webcam");
                _state.IsMicOn = _regListener.CheckAccessFor("microphone");
                if (!_state.IsDirty)
                {
                    Thread.Sleep(500);
                    continue;
                }
                foreach (IListener l in _listeners)
                {
                    l.OnStateChanged(_state);
                }
                _state.IsDirty = false;
            }
        }

    }
}
