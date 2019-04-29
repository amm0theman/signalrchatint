using System;
using System.Windows;
using SignalRChat;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SignalRChatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChatVM chatVM;

        public MainWindow()
        {
            AbstractDispatcher abstractDispatcher = new AbstractDispatcher(this.Dispatcher);
            chatVM = new ChatVM(new ClientHubProxy("http://localhost:8080", "chat"), abstractDispatcher);
            this.DataContext = chatVM;
            InitializeComponent();
        }

        public class AbstractDispatcher : IDispatcher
        {
            Dispatcher dispatcher;

            public AbstractDispatcher(Dispatcher _dispatcher)
            {
                dispatcher = _dispatcher;
            }

            public void Invoke(Action a)
            {
                dispatcher.Invoke(a);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chatVM.CloseConnection();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            chatVM.ChatMessageToSend = "";
        }
    }
}
