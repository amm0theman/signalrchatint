using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SignalRChatWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }


        private void LoginButton(object sender, RoutedEventArgs e)
        {
            //Login button sends the user info to the server to be checked, if confirmed, move to login
            //Else stay and error message
            
        }

        private void SignUpButton(object sender, RoutedCommand e)
        {
            //If send data to server to be saved and encrypted, log in user, then 
        }
    }
}
