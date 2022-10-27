using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace CreativeWritersToolkitApp.Views
{
    public class NfSubCMI
    {
        public string NfSubCHeader { get; set; }
        public ICommand NfSubCCommand { get; set; }
        public object NfSubCCommandParameter { get; set; }
        public IList<NfSubCMI> Items { get; set; }
    }
}