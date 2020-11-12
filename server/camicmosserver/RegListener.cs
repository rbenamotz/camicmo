using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace camicmosserver
{
    class RegListener
    {
        public RegListener()
        {
        }
        private static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
        private List<String> CheckNonPackaged(RegistryKey key)
        {
            var output = new List<String>();
            foreach (string s in key.GetSubKeyNames())
            {
                RegistryKey k1 = key.OpenSubKey(s);
                if (CheckKey(k1))
                {
                    var tmp = s;
                    int p = s.LastIndexOf("#");
                    if (p>0) { tmp = tmp.Substring(p+1); }
                    p = tmp.LastIndexOf(".");
                    if (p > 0) { tmp = tmp.Substring(0,p ); }
                    output.Add(tmp);
                }
            }
            return output;
        }
        public bool CheckAccessFor(String capability)
        {
            var output = WhatIsUsing(capability);
            foreach(var s in output)
            {
                Console.WriteLine(s);
            }
            return (output.Count>0);

        }
        public ICollection<string> WhatIsUsing(String capability)
        {
            var output = new List<String>();
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\" + capability, false); //\\\\\\microphone", false);
            foreach (string s in key.GetSubKeyNames())
            {
                RegistryKey k1 = key.OpenSubKey(s);
                if (s=="NonPackaged")
                {
                    output.AddRange(CheckNonPackaged(k1));
                } 
                int p = s.LastIndexOf("_");
                if (p == -1)
                {
                    continue;
                }
                if (CheckKey(k1))
                {
                    var tmp = s;
                    p = tmp.IndexOf(".");
                    if (p > 0) { tmp = s.Substring(p + 1); }
                    p = tmp.IndexOf("_");
                    if (p>0) { tmp = tmp.Substring(0, p); }
                    tmp = SplitCamelCase(tmp);
                    output.Add(tmp);
                }
            }
            return output;

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
