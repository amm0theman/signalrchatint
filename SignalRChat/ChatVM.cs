using SignalRChat.Aggregates;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SignalRChat
{
    public class ChatVM : ViewModelBase, INotifyDataErrorInfo
    {
        private IClientHubProxy clientHubProxy;

        string localuser;
        IDispatcher UIDispatcher;
        ObservableCollection<string> chatLog = new ObservableCollection<string>();
        ObservableCollection<string> users = new ObservableCollection<string>();
        string chatMessageToSend = "";
        ObservableCollection<string> loginLog = new ObservableCollection<string>();

        #region properties
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

        private ICommand _LoginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _LoginCommand;
            }
            set
            {
                _LoginCommand = value;
            }
        }

        private ICommand _SignUpCommand;
        public ICommand SignUpCommand
        {
            get
            {
                return _SignUpCommand;
            }
            set
            {
                _SignUpCommand = value;
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

        public IDispatcher Dispatcher
        {
            get
            {
                return UIDispatcher;
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

        private string userLogin;
        public string UserLogin
        {
            get
            {
                return userLogin;
            }

            set
            {
                if (value == userLogin)
                    return;
                if (isChatValid(value))
                {
                    userLogin = value;
                }
                NotifyPropertyChanged();
            }
        }

        private string userPass;
        public string UserPass
        {
            get
            {
                return userPass;
            }

            set
            {
                if (value == userPass)
                    return;
                if (isChatValid(value))
                {
                    userPass = value;
                }
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<string> LoginLog
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
        #endregion

        public ChatVM(IClientHubProxy _clientHubProxy, IDispatcher Dispatcher)
        {
            //Needed to get UI dispatcher. would be great to abstract out and get handed the UI dispatcher through constructor injection
            UIDispatcher = Dispatcher;

            //example and the one used in test: clientHubProxy = new ClientHubProxy("http://localhost:8080", "chat");
            clientHubProxy = _clientHubProxy;
            clientHubProxy.startHub();

            //Register events and event handlers
            clientHubProxy.MessageReceived += receivedMessage;
            clientHubProxy.UsernameReceived += receivedUsername;
            clientHubProxy.LogReceived += receivedLog;
            clientHubProxy.UsernamesReceived += receivedUsernames;
            clientHubProxy.LocalUsernameReceived += receivedLocalUser;
            clientHubProxy.UsernameReceivedDisconnect += removeUser;
            var b = new Parcel(new Entities.Message("TestUseBoy"), new User(new Value_Objects.Username("NuttyBoi"), new Value_Objects.Password("")));
            MessageCommand = new SendCommand(new Action<object>((a) => sendMessage(new Parcel(new Entities.Message(ChatMessageToSend), new User(new Value_Objects.Username(LocalUser), new Value_Objects.Password(LocalUser))))));
            GetLogCommand = new SendCommand(new Action<object>((a) => getLog()));
            LoginCommand = new LoginCommand(new Action<object>((a) => getLogin(new User(new Value_Objects.Username(userLogin), new Value_Objects.Password(userPass)))));
            SignUpCommand = new SignUpCommand(new Action<object>((a) => getSignUp(new User(new Value_Objects.Username(LocalUser), new Value_Objects.Password(LocalUser)))));
            
            //Login action so to speak
            setName();            
        }

        public void CloseConnection()
        {
            clientHubProxy.CloseConnection();
        }

        #region events&eventhandlers
        public void receivedMessage(object sender, MessageEventArgs e)
        {
            UIDispatcher.Invoke(new Action(() => chatLog.Add(e.User + ": " + e.Message)));
        }

        public void receivedUsername(object sender, MessageEventArgs e)
        {
            UIDispatcher.Invoke(new Action(() => Users.Add(e.User)));
            UIDispatcher.Invoke(new Action(() => chatLog.Add(e.User + ": " + "Connected")));
        }

        public void removeUser(object sender, MessageEventArgs e)
        {
            UIDispatcher.Invoke(new Action(() => Users.Remove(e.User)));
        }

        public void receivedUsernames(object sender, UsersArgs e)
        {
            UIDispatcher.Invoke(new Action(() =>
            {
                foreach (var username in e.Users)
                    Users.Add(username);
            }));
        }

        public void receivedLog(object sender, LogEventArgs e)
        {
            UIDispatcher.Invoke(new Action(() =>
            {
                foreach (var message in e.Log)
                    LoginLog.Add(message);
            }));

        }

        public void receivedLocalUser(object sender, MessageEventArgs e)
        {
            LocalUser = e.User;
        }

        public void getLog()
        {
            clientHubProxy.getLog();
        }

        public void getLogin(User user)
        {
            clientHubProxy.getLogin(user);
        }

        public void getSignUp(User user)
        {
            clientHubProxy.getSignUp(user);
        }

        public void sendMessage(Parcel message)
        {
            clientHubProxy.sendMessage(message);
            ChatMessageToSend = "";
        }

        public void setName()
        {
            clientHubProxy.setName();
        }
        #endregion

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
            if (value.Contains("shit") | value.Contains("fuck") | value.Contains("damn") | value.Contains("bitch") | value.Contains("Shit") | value.Contains("Fuck") | value.Contains("Damn") | value.Contains("Bitch"))
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

    public interface IDispatcher
    {
        void Invoke(Action a);
    }
}
