using System;
using System.Windows;
using System.Text.RegularExpressions;
using System.Configuration;

namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            int defaultPort = Int32.Parse(ConfigurationManager.AppSettings["port"].ToString());
            string defaultIP = ConfigurationManager.AppSettings["ip"].ToString();
            ipValue.Text = defaultIP;
            portValue.Text = defaultPort.ToString();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = ipValue.Text;
            string port = portValue.Text;
            if (!ValidateIPv4(ip))
            {
                MessageBox.Show("Invalid IP address!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!ValidatePort(port))
            {
                MessageBox.Show("Invalid port number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).Connect(ip, Int32.Parse(port));                
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateIPv4(string ip)
        {
            //regex for a valid IP address
            var regex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
            var match = Regex.Match(ip, regex, RegexOptions.IgnoreCase);

            return match.Success;
        }

        private bool ValidatePort(string ip)
        {
            //regex for a valid TCP/UDP Port, range 1-65535
            var regex = @"^()([1-9]|[1-5]?[0-9]{2,4}|6[1-4][0-9]{3}|65[1-4][0-9]{2}|655[1-2][0-9]|6553[1-5])$";
            var match = Regex.Match(ip, regex, RegexOptions.IgnoreCase);

            return match.Success;
        }
    }
}
