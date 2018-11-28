using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SignalRChat
{
    class SendCommand : ICommand
    {
        private Action<object> _execute = null;

        public SendCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }
    }
}
