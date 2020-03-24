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
        Model model;
        public MainWindow()
        {
            TcpClient tcpClient = new TcpClient();
            model = new Model(tcpClient);

            vm = new ViewModel(model);
            DataContext = vm;

            InitializeComponent();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            model.connect("127.0.0.1", 5402);
            model.start();
            statusValue.Text = "Connected";
            statusValue.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            model.disconnect();
            statusValue.Text = "Disconnected";
            statusValue.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.disconnect();
            statusValue.Text = "Disconnected";
            statusValue.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}
