using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SignalRChatWPF
{
    public class MainWindowVM : INotifyPropertyChanged
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

        public MainWindowVM(int switchview, PropertyChangedEventArgs e)
        {
            SwitchView = switchview;
            PropertyChanged.Invoke("SwitchView", e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
