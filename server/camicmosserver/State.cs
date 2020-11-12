﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace camicmosserver
{
    public class State
    {
        private Dictionary<string, ICollection<String>> _register;
        public const string WEBCAM = "webcam";
        public const string MIC = "microphone";
        public static State Empty = new State();

        public State()
        {
            _register = new Dictionary<string, ICollection<string>>();
            IsDirty = true;
        }
        public bool IsDirty { get; set; }
        public bool IsSomethingOn { get { return (_register.Count > 0); } }

        public bool IsCapbilityOn(string capability)
        {
            return _register.ContainsKey(capability);
        }

        internal void RegisterProgramsFor(string capability, ICollection<string> progs)
        {
            bool b = IsCapbilityOn(capability);
            if (progs == null || progs.Count() == 0)
            {
                _register.Remove(capability);
            }
            else
            {
                _register[capability] = progs;
            }

            IsDirty = IsDirty || (IsCapbilityOn(capability) != b);
        }

        internal IEnumerable<object> ProgsForCapability(string capability)
        {
            if (!_register.ContainsKey(capability))
            {
                return new String[] { };
            }
            return _register[capability];
        }
    }
}
