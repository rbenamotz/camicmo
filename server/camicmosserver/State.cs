using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camicmosserver
{
    class State
    {
        private bool _isMicOn;
        private bool _isCamOn;

        public State(bool isCamOn, bool isMicOn)
        {
            _isCamOn = isCamOn;
            _isMicOn = isMicOn;
            IsDirty = false;
        }
        public bool IsDirty { get; set; }
        public bool IsCamOn {
            get { return _isCamOn; }
            set {
                IsDirty = (IsDirty || value != _isCamOn) ;
                _isCamOn = value;
            }
        }
        public bool IsMicOn
        {
            get { return _isMicOn; }
            set
            {
                IsDirty = (IsDirty ||  value != _isMicOn);
                _isMicOn = value;
            }
        }
    }
}
