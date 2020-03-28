using FlightSimulatorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Threading;

namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        public MainWindow()
        {
            TcpClient tcpClient = new TcpClient();

            vm = new ViewModel(new Model(tcpClient));
            DataContext = vm;

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectWindow window = new ConnectWindow();
            window.ShowDialog();
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.disconnect();
            statusValue.Content = "Disconnected";
            statusValue.Foreground = new SolidColorBrush(Colors.Red);
            latitudeValue.Visibility = Visibility.Hidden;
            latitudeTitle.Visibility = Visibility.Hidden;
            longitudeValue.Visibility = Visibility.Hidden;
            longitudeTitle.Visibility = Visibility.Hidden;
            mapCanvas.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.disconnect();
        }

        internal void Connect(string ip, int port)
        {
            vm.connect(ip, port);
            vm.start();
            statusValue.Content = "Connected";
            statusValue.Foreground = new SolidColorBrush(Colors.Green);
            latitudeValue.Visibility = Visibility.Visible;
            latitudeTitle.Visibility = Visibility.Visible;
            longitudeValue.Visibility = Visibility.Visible;
            longitudeTitle.Visibility = Visibility.Visible;
            mapCanvas.Visibility = Visibility.Visible;
        }

        private void dashboard_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
