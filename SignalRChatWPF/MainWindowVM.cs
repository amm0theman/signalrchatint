using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChatWPF
{
    public class MainWindowVM
    {
        public int SwitchView
        {
            get;
            set;
        }

        public MainWindowVM()
        {
            SwitchView = 0;
        }
    }
}
