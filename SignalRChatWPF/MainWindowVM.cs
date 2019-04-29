using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SignalRChatWPF
{
    public class MainWindowVM
    {
        public int SwitchView
        {
            set;
            get;
        }

        public MainWindowVM()
        {
            SwitchView = 1;                
        }

        public MainWindowVM(int switchview)
        {
            SwitchView = switchview;
        }
    }
}
