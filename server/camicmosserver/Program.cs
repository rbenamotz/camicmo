using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace camicmosserver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var s = new SerialHandler();
            while (true)
            {
                s.SetColors("gg");
                Thread.Sleep(500);
                s.SetColors("gb");
                Thread.Sleep(500);
                s.SetColors("br");
                Thread.Sleep(500);
                s.SetColors("rr");
                Thread.Sleep(500);
                s.SetColors("r ");
                Thread.Sleep(500);
                s.SetColors("  ");
                Thread.Sleep(500);
                s.SetColors(" g");
                Thread.Sleep(500);

            }

        }

        static void RunService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);

        }
    }
}
