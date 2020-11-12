using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver
{
    class RegListener
    {
        public RegListener()
        {
        }

        private bool CheckNonPackaged(RegistryKey key)
        {
            foreach (string s in key.GetSubKeyNames())
            {
                RegistryKey k1 = key.OpenSubKey(s);
                if (CheckKey(k1))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckAccessFor(String capability)
        {
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\" + capability, false); //\\\\\\microphone", false);
            foreach (string s in key.GetSubKeyNames())
            {
                RegistryKey k1 = key.OpenSubKey(s);
                if (s=="NonPackaged")
                {
                    if (CheckNonPackaged(k1))
                    {
                        return true;
                    }
                } 
                int p = s.LastIndexOf("_");
                if (p == -1)
                {
                    continue;
                }
                //String appName = s.Substring(0, p);
                //Console.WriteLine(appName);
                if (CheckKey(k1))
                {
                    return true;
                }
            }
            return false;

        }

        private bool CheckKey(RegistryKey k)
        {
            var lastStop = k.GetValue("LastUsedTimeStop");
            if (lastStop != null)
            {
                bool b = ((long)lastStop == 0);
                if (b)
                {
                    return true;
                }
            }
            return false;

        }
    }
}
