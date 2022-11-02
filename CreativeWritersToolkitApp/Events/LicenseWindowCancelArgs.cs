using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Events
{
    public class LicenseWindowCancelArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}
