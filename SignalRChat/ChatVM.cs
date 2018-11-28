using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SignalRChat
{
    public class ChatVM : ViewModelBase, INotifyDataErrorInfo
    {
        private ClientHubProxy clientHubProxy;

        string localuser;
        ListCollectionView view;
        ObservableCollection<string> chatLog;
        ObservableCollection<string> users;
        string chatMessageToSend;
        string loginLog;

        private ICommand _messageCommand;
        public ICommand MessageCommand
        {
            get
            {
                return _messageCommand;
            }
            set
            {
                _messageCommand = value;
            }
        }

        private ICommand _getLogCommand;
        public ICommand GetLogCommand
        {
            get
            {
                return _getLogCommand;
            }
            set
            {
                _getLogCommand = value;
            }
        }

        public string LocalUser
        {
            get
            {
                return localuser;
            }
            set
            {
                if (value == LocalUser)
                    return;
                localuser = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> ChatLog
        {
            get
            {
                return chatLog;
            }
            set
            {
                if (value == chatLog)
                    return;
                chatLog = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string>  Users
        {
            get
            {
                return users;
            }

            set
            {
                if (value == users)
                    return;
                users = value;
                NotifyPropertyChanged();
            }
        }

        public string ChatMessageToSend
        {
            get
            {
                return chatMessageToSend;
            } 

            set
            {
                if (value == chatMessageToSend)
                    return;
                if (isChatValid(value))
                {
                    chatMessageToSend = value;
                }
                NotifyPropertyChanged();
            }
        }

        public string LoginLog
        {
            get
            {
                return loginLog;
            }

            set
            {
                if (value == loginLog)
                    return;
                loginLog = value;
                NotifyPropertyChanged();
            }
        }

        public ChatVM()
        {
            chatLog = new ObservableCollection<string>();
            users = new ObservableCollection<string>();
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(users);
            chatMessageToSend = "Type Message Here";
            //example and the one used in test: clientHubProxy = new ClientHubProxy("http://localhost:8080", "chat");
            clientHubProxy = new ClientHubProxy("http://localhost:8080", "chat");
            clientHubProxy.startHub();

            //Register events
            clientHubProxy.MessageReceived += receivedMessage;
            clientHubProxy.UsernameReceived += receivedUsername;
            clientHubProxy.LogReceived += receivedLog;
            MessageCommand = new SendCommand(new Action<object>((a) => sendMessage(LocalUser, ChatMessageToSend)));
            GetLogCommand = new SendCommand(new Action<object>((a) => getLog()));

            setName();            
        }

        public void receivedMessage(object sender, MessageEventArgs e)
        {
            view.Dispatcher.Invoke(new Action(() => chatLog.Add(e.User + ": " +e.Message)));
        }

        public void receivedUsername(object sender, MessageEventArgs e)
        {
            view.Dispatcher.Invoke(new Action(() => Users.Add(e.User)));
            LocalUser = e.User;
            sendMessage("ChatBot |O_O|", "User " + LocalUser + "has connected");
        }

        public void receivedLog(object sender, LogEventArgs e)
        {
            loginLog = e.Log;
        }

        public void getLog()
        {
            clientHubProxy.getLog();
        }

        public void sendMessage(string user, string message)
        {
            clientHubProxy.sendMessage(user, message);
        }

        private void setName()
        {
            clientHubProxy.setName();
        }

        #region dataValidation
        private ConcurrentDictionary<string, List<string>> _errors = new ConcurrentDictionary<string, List<string>>();

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
                !_errors.ContainsKey(propertyName)) return null;
            return _errors[propertyName];
        }

        private const string MESSAGE_ERROR = "Messages must not contain profanity.";
        public bool isChatValid(string value)
        {
            bool isValid = true;
            if (value.Contains("shit") | value.Contains("fuck") | value.Contains("damn") | value.Contains("bitch"))
            {
                AddError("ChatMessageToSend", MESSAGE_ERROR, false);
                return isValid = false;
            }
            else RemoveError("ChatMessageToSend", MESSAGE_ERROR);
            return isValid;
        }

        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                if (isWarning) _errors[propertyName].Add(error);
                else _errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (_errors.ContainsKey(propertyName) &&
                _errors[propertyName].Contains(error))
            {
                _errors[propertyName].Remove(error);
                if (_errors[propertyName].Count == 0) _errors.TryRemove(propertyName, out var as1);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
    }
}
