using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SignalRChat;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SignalRChatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AbstractDispatcher abstractDispatcher = new AbstractDispatcher(this.Dispatcher);
            ChatVM chatVM = new ChatVM(new ClientHubProxy("http://localhost:8080", "chat"), abstractDispatcher);
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
    }
}
