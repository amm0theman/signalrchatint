using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SignalRChat
{
    public class ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
