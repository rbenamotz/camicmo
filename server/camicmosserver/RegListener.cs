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
        public ICollection<string> WhatIsUsing(string capability)
        {
            var base1  = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var base2 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            var output = new List<String>();
            output.AddRange(WhatIsUsing(capability, base1));
            output.AddRange(WhatIsUsing(capability, base2));
            return output;
        }

        private IEnumerable<string> WhatIsUsing(string capability, RegistryKey baseKey)
        {
            const string path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\";
            var output = new List<String>();
            //var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var key = baseKey.OpenSubKey (path + capability, false); //\\\\\\microphone", false);
            if (key==null)
            {
                return output;
            }
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
            var lastStart = k.GetValue("LastUsedTimeStart") as long?;
            var lastStop = k.GetValue("LastUsedTimeStop") as long?;
            if (lastStop != null && lastStart!=null)
            {
                var ts = lastStart - lastStop;
                if (ts>0)
                {
                    return true;
                }
            }
            return false;

        }
    }
}
